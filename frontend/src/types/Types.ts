import type { StaticImageData } from 'next/image';

export type ImageWithAlt = {
  data: StaticImageData;
  alt: string;
};

export interface PredictionResultType {
  predictedLabel: string;
  confidence: number;
  scores: number[];
}

export type ModelImagesType = ImageInfo[];

export interface ToolImageType {
    id: number;
    src: string; // The URL from Cloudinary
    alt: string;
}

export interface ToolType {
  id: number;
  icon: string;
  title: string;
  description: string | null;
  href: string | null;
  toolImages: ToolImageType[];
}

export interface RegressionResult {
    predictedLabel: string;
    predictedValue: number;
}

export interface ClassificationResult {
    predictedLabel: string;
    confidence: number;
    scores: number[];
}

export type PredictionResult = ClassificationResult | RegressionResult;

export type PredictionModelType = {
  id: number;
  modelName: string;
  version: string;
  description: string | null;
  filePath: string | null;
  framework: string;
  inputShape: string | null;
  numClasses: number;
  labels: string | null;
  createdAt: string;
  accuracy: number;
};

export type ImageInfo = {
    id: string | number;
    src: string; // Corrected to only accept a string (the publicId)
    alt: string;
};

