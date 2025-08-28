"use client";

import React, { useState } from 'react';
import ImageUploader from './ImageUploader';
import PredictionResult from './PredictionResult';
import ModelStats from './ModelStats';

// --- MOCK DATA (Normalde API'dan gelecek) ---
const mockModelDetails = {
  name: "Beyin Tümörü Sınıflandırması (ResNet50)",
  version: "v1.3.2",
  description: "Bu model, ResNet50 mimarisi kullanılarak eğitilmiştir ve dört ana beyin tümörü tipini (Meningioma, Glioma, Pituiter Tümör) ve tümör olmayan MR görüntülerini yüksek doğrulukla sınıflandırmak için tasarlanmıştır.",
  inputShape: "224x224x3",
  labels: ["Meningioma", "Glioma", "Pituiter Tümör", "Tümör Yok"],
  totalPredictions: 14823, // Bu modelin toplam kaç kez kullanıldığı
  accuracy: 0.981, // Modelin genel doğruluk oranı
};

type Prediction = {
  label: string;
  confidence: number;
};

// --- Bileşen Başlıyor ---
const ModelDetailPage = ({ params }: { params: { modelId: string } }) => {
  // Sayfanın state'lerini burada yönetiyoruz
  const [uploadedImage, setUploadedImage] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(false);
  const [prediction, setPrediction] = useState<Prediction[] | null>(null);

  // Dosya yüklendiğinde ve analiz başladığında tetiklenecek fonksiyon
  const handleAnalysis = (file: File) => {
    const reader = new FileReader();
    reader.onloadend = () => {
      setUploadedImage(reader.result as string);
      setIsProcessing(true);
      setPrediction(null);

      // --- MOCK API ÇAĞRISI (Simülasyon) ---
      // Gerçekte burada backend'e istek atılacak
      setTimeout(() => {
        // Rastgele ama tutarlı sonuçlar üreten mock sonuç
        const mockResults: Prediction[] = mockModelDetails.labels.map(label => ({
          label,
          confidence: Math.random(),
        }));
        // Toplamı 1 olacak şekilde normalize et
        const total = mockResults.reduce((acc, r) => acc + r.confidence, 0);
        mockResults.forEach(r => r.confidence /= total);
        mockResults.sort((a, b) => b.confidence - a.confidence);
        
        setPrediction(mockResults);
        setIsProcessing(false);
      }, 3000); // 3 saniyelik bir bekleme simülasyonu
    };
    reader.readAsDataURL(file);
  };

  return (
    <div className="min-h-screen p-4 sm:p-6 lg:p-8">
      {/* Model Başlığı */}
      <div className="mb-8">
        <h1 className="text-3xl sm:text-4xl font-bold text-text-primary tracking-tight">{mockModelDetails.name}</h1>
        <p className="mt-2 text-md text-text-secondary">{mockModelDetails.description}</p>
      </div>

      {/* Üst Kısım: Yükleme ve Sonuç */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-12">
        <ImageUploader onAnalyze={handleAnalysis} isProcessing={isProcessing} />
        <PredictionResult result={prediction} isProcessing={isProcessing} labels={mockModelDetails.labels} />
      </div>

      {/* Alt Kısım: İstatistikler */}
      <ModelStats stats={mockModelDetails} />
    </div>
  );
};

export default ModelDetailPage;