# preprocess.py

import os
import pandas as pd
import nibabel as nib
import numpy as np
from tqdm import tqdm
from pathlib import Path
from skimage.transform import resize
import shutil
import imageio.v2 as imageio  # PNG için

# --- 1. SABİTLER ---
TARGET_SIZE = (224, 224)
mri_base_path = "../Data/MultipleSclerosisMRI"  # kendi path'inizi buraya girin

output_dir = Path("Processed_Data")
if output_dir.exists():
    print(f"'{output_dir}' klasörü zaten var, içi temizleniyor...")
    shutil.rmtree(output_dir)
output_dir.mkdir(parents=True, exist_ok=True)
print(f"İşlenmiş veriler '{output_dir}' klasörüne kaydedilecek.")

# Metadata yükleme
try:
    patient_folders = sorted([f for f in os.listdir(mri_base_path) if f.startswith("Patient")])
    clinical_df = pd.read_excel(os.path.join(mri_base_path, "InfoPatient.xlsx"), header=1)
    clinical_df.rename(columns={"ID": "PatientID"}, inplace=True)
    full_metadata = clinical_df.dropna(subset=['EDSS']).copy()
    print(f"{len(patient_folders)} hasta klasörü ve {len(full_metadata)} geçerli EDSS skoru bulundu.")
except Exception as e:
    print(f"Metadata veya hasta klasörleri okunurken hata oluştu: {e}")
    exit()

# --- 2. YARDIMCI FONKSİYONLAR ---

def robust_normalize(img_slice):
    img_slice = img_slice.astype(np.float32)
    p1, p99 = np.percentile(img_slice, (1, 99))
    if p99 > p1:
        img_slice = np.clip(img_slice, p1, p99)
        img_slice = (img_slice - p1) / (p99 - p1)
    return img_slice

def create_multi_modal_input_and_resize(flair_slice, t1_slice, t2_slice):
    flair_norm = robust_normalize(flair_slice)
    t1_norm = robust_normalize(t1_slice)
    t2_norm = robust_normalize(t2_slice)
    multi_modal_slice = np.stack([flair_norm, t1_norm, t2_norm], axis=-1)
    resized_slice = resize(multi_modal_slice, (*TARGET_SIZE, 3), preserve_range=True)
    return resized_slice.astype(np.float32)

def load_and_resample_modalities(seg_img, flair_img, t1_img, t2_img):
    """Modaliteleri segmentation boyutuna göre resample et."""
    target_shape = seg_img.shape  # (x,y,z)

    def resample_to_target(img, target_shape):
        data = img.get_fdata(dtype=np.float32)
        if data.shape != target_shape:
            data = resize(
                data,
                target_shape,
                preserve_range=True,
                anti_aliasing=True
            )
        return data

    flair_resampled = resample_to_target(flair_img, target_shape)
    t1_resampled = resample_to_target(t1_img, target_shape)
    t2_resampled = resample_to_target(t2_img, target_shape)

    return flair_resampled, t1_resampled, t2_resampled

# --- 3. ANA DÖNGÜ ---

processed_records = []
total_slices_saved = 0

print("\nÖn işleme başlıyor...")
for folder_name in tqdm(patient_folders, desc="Hastalar işleniyor"):
    pid = int(folder_name.split("-")[-1])

    if pid not in full_metadata['PatientID'].values:
        continue

    edss_score_row = full_metadata.loc[full_metadata["PatientID"] == pid, "EDSS"]
    if edss_score_row.empty:
        continue
    edss_score = edss_score_row.values[0]

    try:
        folder_path = os.path.join(mri_base_path, folder_name)

        seg_path = os.path.join(folder_path, f"{pid}-LesionSeg-Flair.nii")
        flair_path = os.path.join(folder_path, f"{pid}-Flair.nii")
        t1_path = os.path.join(folder_path, f"{pid}-T1.nii")
        t2_path = os.path.join(folder_path, f"{pid}-T2.nii")

        if not all(os.path.exists(p) for p in [seg_path, flair_path, t1_path, t2_path]):
            print(f"Uyarı: Hasta {pid} için dosyalardan biri eksik, atlanıyor.")
            continue

        seg_img = nib.load(seg_path)
        flair_img = nib.load(flair_path)
        t1_img = nib.load(t1_path)
        t2_img = nib.load(t2_path)

        # Modaliteleri aynı shape'e getir
        flair_data, t1_data, t2_data = load_and_resample_modalities(seg_img, flair_img, t1_img, t2_img)
        seg_data = seg_img.get_fdata(dtype=np.float32)

        # Lezyonlu slice’lar
        lesion_slices_indices = [i for i in range(seg_data.shape[2]) if np.any(seg_data[:, :, i])]
        if not lesion_slices_indices:
            mid = seg_data.shape[2] // 2
            lesion_slices_indices = [max(0, mid-2), mid, min(seg_data.shape[2]-1, mid+2)]

        for slice_idx in lesion_slices_indices:
            flair_slice = flair_data[..., slice_idx]
            t1_slice = t1_data[..., slice_idx]
            t2_slice = t2_data[..., slice_idx]

            processed_slice = create_multi_modal_input_and_resize(flair_slice, t1_slice, t2_slice)

            # PNG dosya adı
            slice_filename = f"patient_{pid}_slice_{slice_idx}.png"
            slice_filepath = output_dir / slice_filename

            # [0,255] aralığına getir ve kaydet
            img_uint8 = (processed_slice * 255).clip(0, 255).astype(np.uint8)
            imageio.imwrite(slice_filepath, img_uint8)

            processed_records.append({
                'filepath': str(slice_filepath),
                'patient_id': pid,
                'edss': edss_score
            })
            total_slices_saved += 1

    except Exception as e:
        print(f"Hata: Hasta {pid} işlenirken bir sorun oluştu: {e}")
        continue

# --- 4. METADATA KAYDETME ---
if not processed_records:
    print("Hiçbir dilim işlenemedi. Lütfen dosya yollarını ve formatları kontrol edin.")
else:
    metadata_df = pd.DataFrame(processed_records)
    metadata_filepath = "processed_metadata.csv"
    metadata_df.to_csv(metadata_filepath, index=False)
    print("\n--- Ön İşleme Tamamlandı! ---")
    print(f"Toplam {total_slices_saved} adet dilim işlenip kaydedildi.")
    print(f"Metadata '{metadata_filepath}' dosyasına yazıldı.")
    print(metadata_df.head())
