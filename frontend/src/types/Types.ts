import type { StaticImageData } from 'next/image';

export type ImageWithAlt = {
  data: StaticImageData;
  alt: string;
};

export interface PredictionResultType {
  prediction: number[]; 
}

export type ModelImagesType = ImageInfo[];

export interface ToolImageType {
  id: number;
  src: string;
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
};

export type ImageInfo = {
  id: string | number;
  src: string | StaticImageData;
  alt: string;
};

