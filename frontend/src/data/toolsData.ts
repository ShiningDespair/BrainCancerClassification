import { Brain, Stethoscope, ChartLineUp } from '@phosphor-icons/react';
import { alzheimerImages, brainTumorImages, msImages } from './imageData';
import { ToolType } from '@/types/Types';

export const toolsData: ToolType[] = [
  {
    icon: Brain,
    title: 'Beyin Tümörü Sınıflandırma Modeli (ResNet50)',
    description: 'Glioma, meningioma ve pituitary gibi tümör türlerini sağlıklı beyin dokusundan ayırt etmek için geliştirilmiştir. Transfer öğrenme yaklaşımı ile ResNet50 mimarisi kullanılmıştır. Eğitim sırasında karşılaşılan VRAM yetersizliği gibi donanımsal zorluklar, "Mixed Precision" ve görüntü boyutunu optimize etme gibi ileri tekniklerle aşılarak modelin performansı korunmuştur.',
    href: '/modeller/beyin-tumoru',
    images: brainTumorImages,
  },
  {
    icon: Stethoscope,
    title: 'Alzheimer Evrelerini Sınıflandırma Modeli (Baseline CNN)',
    description: 'Alzheimer hastalığının farklı evrelerini (Hafif, Orta, Çok Hafif ve Demans Olmayan) sınıflandırmak üzere tasarlanmıştır. Veri setindeki ciddi sınıf dengesizliği sorununa çözüm olarak, standart modeller yerine sıfırdan özel bir Evrişimli Sinir Ağı (CNN) mimarisi oluşturulmuş ve veri seti SMOTE tekniği ile zenginleştirilerek modelin ezberlemesi engellenmiş ve doğruluk oranı artırılmıştır.',
    href: '/modeller/alzheimer',
    images: alzheimerImages,
  },
  {
    icon: ChartLineUp,
    title: 'Multiple Sclerosis (MS) EDSS Skor Tahmini (Regresyon)',
    description: 'Bu model, bir sınıflandırma problemi yerine, MS hastalarının MR görüntüleri ve demografik verilerini kullanarak Genişletilmiş Engellilik Durum Ölçeği (EDSS) skorunu tahmin eden bir regresyon problemidir. Kısıtlı veri setine rağmen hem derin öğrenme (ResNet50) hem de klasik makine öğrenmesi (XGBoost) yaklaşımları denenerek, veri odaklı problemlere yönelik kapsamlı bir analiz sunulmuştur.',
    href: '/modeller/ms-edss-tahmini',
    images: msImages,
  },
];