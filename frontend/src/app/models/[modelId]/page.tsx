"use client";

import React, { useState, useEffect, use } from 'react';
import ImageUploader from './ImageUploader';
import PredictionResult from './PredictionResult';
import ModelStats from './ModelStats';
import { getPredictionModel, predictImage } from '@/services/apiService';
import { PredictionModelType, ClassificationResult, RegressionResult } from '@/types/Types';

type FormattedPrediction = {
  label: string;
  confidence: number;
};

const parseLabels = (labelsString: string | null | undefined): string[] => {
  if (!labelsString) return [];
  try {
    const labelsArray = JSON.parse(labelsString);
    if (Array.isArray(labelsArray)) {
      return labelsArray;
    }
  } catch (e) {
  }
  return labelsString.split(',').map(label => label.trim());
};

const ModelDetailPage = ({ params: paramsPromise }: { params: Promise<{ modelId: string }> }) => {
  const params = use(paramsPromise);

  const [modelDetails, setModelDetails] = useState<PredictionModelType | null>(null);
  const [uploadedImage, setUploadedImage] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(false);
  const [prediction, setPrediction] = useState<FormattedPrediction[] | null>(null);
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
      const result = await predictImage(params.modelId, file);
      
      if ('scores' in result) {
        const classificationResult = result as ClassificationResult;
        const labels = parseLabels(modelDetails?.labels);
        
        const formattedPrediction = labels.map((label: string, index: number) => ({
          label,
          confidence: classificationResult.scores[index],
        }));
        
        formattedPrediction.sort((a: FormattedPrediction, b: FormattedPrediction) => b.confidence - a.confidence);
        setPrediction(formattedPrediction);

      } else if ('predictedValue' in result) {
        const regressionResult = result as RegressionResult;
        const formattedPrediction = [{
          label: regressionResult.predictedLabel,
          confidence: regressionResult.predictedValue,
        }];
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
    return <div className="min-h-screen mt-20 p-8 text-red-500">{error}</div>;
  }

  if (!modelDetails) {
    return <div className="min-h-screen mt-20 p-8">Loading model details...</div>;
  }
  
  const stats = {
    version: modelDetails.version,
    inputShape: modelDetails.inputShape || "",
    accuracy: modelDetails.accuracy || 0,
  }

  return (
    <div className="min-h-screen mt-20 p-4 sm:p-6 lg:p-8">
      <div className="mb-8">
        <h1 className="text-3xl sm:text-4xl font-bold text-text-primary tracking-tight">{modelDetails.modelName}</h1>
        <p
          className="mt-2 text-md text-text-secondary"
          dangerouslySetInnerHTML={{ __html: modelDetails.description || '' }}
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-12">
        <ImageUploader onAnalyze={handleAnalysis} isProcessing={isProcessing} />
        <PredictionResult 
          result={prediction} 
          isProcessing={isProcessing} 
          labels={parseLabels(modelDetails.labels)} 
          isRegression={modelDetails.numClasses === 1}
        />
      </div>

      <div className="mt-12">
        <ModelStats stats={stats} />
      </div>
    </div>
  );
};

export default ModelDetailPage;