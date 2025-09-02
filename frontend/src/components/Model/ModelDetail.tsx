"use client"

import React from 'react';
import { Swiper, SwiperSlide } from 'swiper/react';
import { Navigation, Scrollbar, FreeMode } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/scrollbar';
import 'swiper/css/free-mode';
import { CldImage } from 'next-cloudinary'; // 💡 Import CldImage
import { ModelImagesType } from '@/types/Types';

type ModelDetailProps = {
  modelImages: ModelImagesType;
};

const ModelDetail: React.FC<ModelDetailProps> = ({ modelImages }) => {
  if (!modelImages || modelImages.length === 0) {
    return null;
  }

  return (
    <Swiper
      modules={[FreeMode, Navigation, Scrollbar]}
      spaceBetween={16}
      slidesPerView={'auto'}
      freeMode={true}
      navigation={true}
      scrollbar={{ draggable: true }}
      className="py-4"
    >
      {modelImages.map((imageInfo) => (
        <SwiperSlide key={imageInfo.id} style={{ width: '18rem' }}> 
          <div className="relative aspect-square w-full">
            <CldImage 
              src={imageInfo.src} 
              alt={imageInfo.alt}
              fill
              className="rounded-lg object-cover"
              sizes="(max-width: 768px) 50vw, 33vw"
            />
          </div>
        </SwiperSlide>
      ))}
    </Swiper>
  );
};

export default ModelDetail;