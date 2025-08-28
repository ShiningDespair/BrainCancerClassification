import type { StaticImageData } from 'next/image';

export type ImageWithAlt = {
  data: StaticImageData;
  alt: string;
};

export type ModelImagesType = ImageInfo[];

export type ToolType = {
  icon: React.ElementType;
  title: string;
  description: string;
  href: string;
  images?: ModelImagesType; 
};

export type ImageInfo = {
  id: string | number;
  src: string | StaticImageData; 
  alt: string;
};

