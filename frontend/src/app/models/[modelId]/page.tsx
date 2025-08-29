"use client";

import React, { useState, useEffect, use } from 'react';
import ImageUploader from './ImageUploader';
import PredictionResult from './PredictionResult';
import ModelStats from './ModelStats';
import { getPredictionModel, uploadImage } from '@/services/apiService';
import { PredictionModelType } from '@/types/Types';

type Prediction = {
  label: string;
  confidence: number;
};

const parseLabels = (labelsString: string | null | undefined): string[] => {
  if (!labelsString) return [];
  try {
    return JSON.parse(labelsString);
  } catch (e) {
    const correctedString = `[${labelsString.replace(/'/g, '"')}]`;
    try {
      return JSON.parse(correctedString);
    } catch (finalError) {
      return [];
    }
  }
};

const ModelDetailPage = ({ params: paramsPromise }: { params: Promise<{ modelId: string }> }) => {
  const params = use(paramsPromise);

  const [modelDetails, setModelDetails] = useState<PredictionModelType | null>(null);
  const [uploadedImage, setUploadedImage] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(false);
  const [prediction, setPrediction] = useState<Prediction[] | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchModelDetails = async () => {
      try {
        const data = await getPredictionModel(params.modelId);
        setModelDetails(data);
      } catch (err) {
        setError('Model details could not be loaded.');
        console.error(err);
      }
    };

    fetchModelDetails();
  }, [params.modelId]);

  const handleAnalysis = async (file: File) => {
    const reader = new FileReader();
    reader.onloadend = () => {
      setUploadedImage(reader.result as string);
    };
    reader.readAsDataURL(file);

    setIsProcessing(true);
    setPrediction(null);
    setError(null);

    try {
      const result = await uploadImage(params.modelId, file);
      
      if (modelDetails && modelDetails.labels) {
        const labels = parseLabels(modelDetails.labels);
        const formattedPrediction = labels.map((label: string, index: number) => ({
          label,
          confidence: result.prediction[index],
        }));
        
        formattedPrediction.sort((a: Prediction, b: Prediction) => b.confidence - a.confidence);
        
        setPrediction(formattedPrediction);
      }
    } catch (err) {
      let errorMessage = 'Prediction failed. Please try again.';
      if (err instanceof Error) {
        errorMessage = err.message;
      }
      setError(errorMessage);
      console.error(err);
    } finally {
      setIsProcessing(false);
    }
  };

  if (error && !isProcessing) {
    return <div className="min-h-screen p-8 text-red-500">{error}</div>;
  }

  if (!modelDetails) {
    return <div className="min-h-screen p-8">Loading model details...</div>;
  }
  
  const stats = {
    accuracy: 0, 
    totalPredictions: 0,
    name: modelDetails.modelName,
    version: modelDetails.version,
    description: modelDetails.description || "",
    inputShape: modelDetails.inputShape || "",
    labels: parseLabels(modelDetails.labels),
  }

  return (
    <div className="min-h-screen pt-24 p-4 sm:p-6 lg:p-8">
      <div className="mb-8">
        <h1 className="text-3xl sm:text-4xl font-bold text-text-primary tracking-tight">{modelDetails.modelName}</h1>
        <p
          className="mt-2 text-md text-text-secondary"
          dangerouslySetInnerHTML={{ __html: modelDetails.description || '' }}
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-12">
        <ImageUploader onAnalyze={handleAnalysis} isProcessing={isProcessing} />
        <PredictionResult result={prediction} isProcessing={isProcessing} labels={parseLabels(modelDetails.labels)} />
      </div>

      <div className="mt-12">
        <ModelStats stats={stats} />
      </div>
    </div>
  );
};

export default ModelDetailPage;