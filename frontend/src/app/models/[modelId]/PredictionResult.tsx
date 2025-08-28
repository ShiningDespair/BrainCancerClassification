"use client";

import React from 'react';
import { ChartBar, Hourglass } from '@phosphor-icons/react';

type Prediction = {
  label: string;
  confidence: number;
};

type Props = {
  result: Prediction[] | null;
  isProcessing: boolean;
  labels: string[];
};

const PredictionResult = ({ result, isProcessing, labels }: Props) => {
  const renderContent = () => {
    if (isProcessing) {
      return (
        <div className="text-center">
          <Hourglass size={48} className="mb-4 text-accent animate-spin" />
          <p className="font-semibold text-lg">Yapay Zeka Analiz Ediyor...</p>
          <p className="text-text-secondary">Bu işlem birkaç saniye sürebilir.</p>
        </div>
      );
    }

    if (!result) {
      return (
        <div className="text-center">
          <ChartBar size={48} className="mb-4 text-text-secondary" />
          <p className="font-semibold text-lg text-text-secondary">Analiz sonucu burada görünecek</p>
        </div>
      );
    }
    
    // En yüksek skoru bul
    const topResult = result[0];

    return (
      <div>
        <div className="mb-6 p-4 rounded-lg bg-accent/10 border border-accent">
          <p className="text-sm text-accent">En Yüksek Olasılık</p>
          <p className="text-2xl font-bold text-text-primary">{topResult.label}</p>
          <p className="text-lg text-text-primary">Güven Skoru: %{(topResult.confidence * 100).toFixed(2)}</p>
        </div>
        
        <h3 className="font-semibold mb-3 text-text-primary">Tüm Olasılıklar</h3>
        <div className="space-y-3">
          {result.map((prediction, index) => (
            <div key={index}>
              <div className="flex justify-between mb-1 text-sm">
                <span className="text-text-primary">{prediction.label}</span>
                <span className="text-text-secondary">%{(prediction.confidence * 100).toFixed(2)}</span>
              </div>
              <div className="w-full bg-primary/50 rounded-full h-2.5">
                <div 
                  className={`rounded-full h-2.5 ${prediction.label === topResult.label ? 'bg-accent' : 'bg-border'}`}
                  style={{ width: `${prediction.confidence * 100}%` }}
                ></div>
              </div>
            </div>
          ))}
        </div>
      </div>
    );
  };

  return (
    <div className="bg-secondary/50 rounded-2xl p-6 h-full flex flex-col justify-center border border-border">
      <h2 className="text-2xl font-semibold text-text-primary mb-4">Analiz Sonucu</h2>
      <div className="flex-grow flex items-center justify-center">
        {renderContent()}
      </div>
    </div>
  );
};

export default PredictionResult;