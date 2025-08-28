# preprocess.py

import os
import pandas as pd
import nibabel as nib
import numpy as np
import cv2  # skimage'dan daha hızlı olabilir
from tqdm import tqdm
from pathlib import Path

# --- AYARLAR ---
# Verilerinizin bulunduğu ana dizin
MRI_BASE_PATH = Path("../Data/MultipleSclerosisMRI")

# İşlenmiş dilimlerin ve yeni metadata'nın kaydedileceği yer
OUTPUT_DIR = MRI_BASE_PATH / "preprocessed_data"
SLICES_DIR = OUTPUT_DIR / "slices"
METADATA_CSV_PATH = OUTPUT_DIR / "slices_metadata.csv"

# Görüntü ayarları
TARGET_SIZE = (224, 224)

# --- KLASÖRLERİ OLUŞTUR ---
SLICES_DIR.mkdir(parents=True, exist_ok=True)

def normalize(img):
    """Görüntü piksellerini 0-1 aralığına normalize eder."""
    img = img.astype(np.float32)
    min_val, max_val = np.min(img), np.max(img)
    return (img - min_val) / (max_val - min_val) if max_val > min_val else img

def main():
    """Ana ön işleme fonksiyonu."""
    print("--- Ön İşleme Başlatıldı ---")
    
    # 1. Hasta listesini ve klinik verileri yükle
    patient_folders = sorted([f for f in os.listdir(MRI_BASE_PATH) if f.startswith("Patient")])
    clinical_df = pd.read_excel(MRI_BASE_PATH / "InfoPatient.xlsx", header=1)
    clinical_df.rename(columns={"ID": "PatientID"}, inplace=True)

    # 2. Tüm hastalar için lezyon hacimlerini hesapla
    patient_volumes = []
    for folder in tqdm(patient_folders, desc="Lezyon Hacimleri Hesaplanıyor"):
        pid = int(folder.split("-")[-1])
        try:
            seg_path = MRI_BASE_PATH / folder / f"{pid}-LesionSeg-Flair.nii"
            seg_data = nib.load(seg_path).get_fdata()
            lesion_voxel_count = np.count_nonzero(seg_data)
            patient_volumes.append({'PatientID': pid, 'LesionVoxelCount': lesion_voxel_count})
        except FileNotFoundError:
            continue
    volume_df = pd.DataFrame(patient_volumes)

    # 3. Tüm meta verileri birleştir ve sınıf etiketlerini oluştur
    full_metadata = pd.merge(clinical_df, volume_df, on="PatientID", how="inner")
    
    # Lezyon hacmine göre dinamik eşikleri ve sınıfları belirle
    _, thresholds = pd.qcut(full_metadata['LesionVoxelCount'], q=3, labels=False, retbins=True, duplicates='drop')
    full_metadata['label_class'] = pd.cut(full_metadata['LesionVoxelCount'], bins=thresholds, labels=False, include_lowest=True)
    
    print("Sınıf dağılımı (Hasta Seviyesinde):\n", full_metadata['label_class'].value_counts())

    # 4. Anlamlı dilimleri ayıkla, işle ve kaydet
    all_slices_metadata = []
    for _, row in tqdm(full_metadata.iterrows(), total=len(full_metadata), desc="Dilimler Ayıklanıyor ve Kaydediliyor"):
        pid = row['PatientID']
        label_class = row['label_class']
        folder = f"Patient-{pid}"
        
        try:
            flair_path = MRI_BASE_PATH / folder / f"{pid}-Flair.nii"
            seg_path = MRI_BASE_PATH / folder / f"{pid}-LesionSeg-Flair.nii"
            
            flair_data = normalize(nib.load(flair_path).get_fdata())
            seg_data = nib.load(seg_path).get_fdata()

            for i in range(flair_data.shape[2]):
                if np.count_nonzero(seg_data[:, :, i]) > 0:
                    slice_2d = flair_data[:, :, i]
                    resized_slice = cv2.resize(slice_2d, TARGET_SIZE)
                    
                    # Dosya adını ve yolunu oluştur
                    slice_filename = f"patient_{pid}_slice_{i}.npy"
                    slice_filepath = SLICES_DIR / slice_filename
                    
                    # Dilimi .npy olarak kaydet (kalite kaybı olmaz)
                    np.save(slice_filepath, resized_slice)
                    
                    # Yeni metadata listesine ekle
                    all_slices_metadata.append({
                        'slice_path': str(slice_filepath),
                        'PatientID': pid,
                        'label_class': label_class
                    })
        except Exception as e:
            print(f"Hasta {pid} işlenirken hata: {e}")
            continue
            
    # 5. Yeni dilim metadata'sını CSV olarak kaydet
    slices_df = pd.DataFrame(all_slices_metadata)
    slices_df.to_csv(METADATA_CSV_PATH, index=False)
    
    print("\n--- Ön İşleme Tamamlandı! ---")
    print(f"{len(slices_df)} adet anlamlı dilim ayıklandı ve '{SLICES_DIR}' klasörüne kaydedildi.")
    print(f"Yeni metadata dosyası '{METADATA_CSV_PATH}' olarak oluşturuldu.")
    print("\nDilim seviyesinde sınıf dağılımı:\n", slices_df['label_class'].value_counts())


if __name__ == '__main__':
    main()

