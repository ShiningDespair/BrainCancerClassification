// frontend/src/app/Homepage/HomepageContent.tsx

"use client";
import React, { useEffect, useState } from 'react';
import { ArrowRight } from '@phosphor-icons/react';
import { getTools } from '@/services/apiService';
import { ToolType } from '@/types/Types';
import ModelDetail from '@/components/Model/ModelDetail';
import * as PhosphorIcons from '@phosphor-icons/react';
import { CldImage } from 'next-cloudinary';
import { getCloudinaryPublicId } from '@/utils/cloudinaryUtils';

const HomePage = () => {
  const [tools, setTools] = useState<ToolType[]>([]);

  useEffect(() => {
    const fetchTools = async () => {
      try {
        const data = await getTools();
        setTools(data);
      } catch (error) {
        console.error("Failed to fetch tools:", error);
      }
    };
    fetchTools();
  }, []);

  const renderIcon = (iconName: string) => {
    const IconComponent = PhosphorIcons[iconName as keyof typeof PhosphorIcons];

    if (IconComponent && typeof IconComponent === 'function') {
      const ValidIcon = IconComponent as React.ElementType;
      return <ValidIcon className="h-8 w-8 text-text-secondary mr-4" weight="light" />;
    }
    
    return null; 
  };

  return (
    <div className="min-h-screen">
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 sm:py-24">

        <section className="text-center mb-24 pt-16">
          <h1 className="text-4xl sm:text-5xl lg:text-6xl font-extrabold text-text-primary tracking-tight">
            Yapay Zeka Destekli Medikal Görüntü Analizi
          </h1>
          <p className="mt-6 max-w-3xl mx-auto text-lg sm:text-xl text-text-secondary">
            Farklı hastalıklara ait MR görüntülerinin derin öğrenme ile sınıflandırılması ve analiz edilmesi için geliştirilmiş bir platform. Her model, kendine özgü zorluklara özel çözümlerle eğitilmiştir.
          </p>
          <div className="mt-8 flex justify-center gap-4">
            <a
              href="#modeller"
              className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-primary bg-text-primary hover:bg-opacity-90 transition"
            >
              Modelleri İncele
            </a>
            <a
              href="/hakkinda"
              className="inline-flex items-center justify-center px-6 py-3 border border-border text-base font-medium rounded-md text-text-primary bg-transparent hover:bg-secondary transition"
            >
              Proje Hakkında
            </a>
          </div>
        </section>

        <section id="modeller">
          <div className="text-center mb-16">
            <h2 className="text-3xl font-bold tracking-tight text-text-primary sm:text-4xl">
              Geliştirilen Analiz Modelleri
            </h2>
            <p className="mt-4 text-lg text-text-secondary">
              Her model, belirli bir hastalığı analiz etmek için özel olarak tasarlanmıştır.
            </p>
          </div>

          <div className="space-y-20">
            {tools.map((tool) => (
              <div key={tool.title}>
                <div className="flex items-center mb-4">
                  {renderIcon(tool.icon)}
                  <h3 className="text-2xl font-semibold text-text-primary">{tool.title}</h3>
                </div>
                <p className="text-text-secondary text-base leading-relaxed mb-6 ml-12">
                  {tool.description}
                </p>

                <div className="ml-12">
                  <div className="border-2 border-dashed border-border rounded-lg p-4">
                    {tool.toolImages && tool.toolImages.length > 0 ? (
                      <ModelDetail
                        modelImages={tool.toolImages.map(img => {
                          const publicId = getCloudinaryPublicId(img.src);
                          return { id: img.id, src: publicId || '', alt: img.alt || '' };
                        })}
                      />
                    ) : (
                      <div className="p-8 text-center">
                        <p className="text-sm text-text-secondary">
                          [Bu model için gösterilecek resim bulunmamaktadır.]
                        </p>
                      </div>
                    )}
                  </div>

                  <a href={tool.href || '#'} className="inline-flex items-center text-sm font-medium text-text-secondary hover:text-text-primary mt-4 group">
                    Daha Fazla Bilgi
                    <ArrowRight className="ml-1 h-4 w-4 transform group-hover:translate-x-1 transition-transform" />
                  </a>
                </div>
              </div>
            ))}
          </div>
        </section>
      </main>
    </div>
  );
};

export default HomePage;