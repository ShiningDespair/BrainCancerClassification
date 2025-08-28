import React from 'react';
// Hatalı isim 'BarChart' yerine doğru isim olan 'ChartBar' kullanıldı.
import { Tag, Cube, ChartBar, CheckCircle } from '@phosphor-icons/react';

type Props = {
  stats: {
    version: string;
    inputShape: string;
    totalPredictions: number;
    accuracy: number;
  };
};

const ModelStats = ({ stats }: Props) => {
  const statItems = [
    { icon: Tag, label: "Model Versiyonu", value: stats.version },
    { icon: Cube, label: "Girdi Boyutu", value: stats.inputShape },
    // Hatalı isim 'BarChart' yerine doğru isim olan 'ChartBar' kullanıldı.
    { icon: ChartBar, label: "Toplam Analiz", value: stats.totalPredictions.toLocaleString('tr-TR') },
    { icon: CheckCircle, label: "Doğruluk Oranı", value: `%${(stats.accuracy * 100).toFixed(1)}` },
  ];

  return (
    <div>
      <h2 className="text-3xl font-bold text-text-primary mb-6">Model İstatistikleri</h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {statItems.map((item, index) => (
          <div key={index} className="bg-secondary/50 rounded-2xl p-6 border border-border">
            <div className="flex items-center text-text-secondary mb-2">
              <item.icon size={18} className="mr-2" />
              <h3 className="text-sm font-medium">{item.label}</h3>
            </div>
            <p className="text-3xl font-semibold text-text-primary">{item.value}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ModelStats;