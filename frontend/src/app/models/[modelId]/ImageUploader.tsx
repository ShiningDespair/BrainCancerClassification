"use client";

import React, { useState, useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
import { CloudArrowUp, Brain } from '@phosphor-icons/react';
import Image from 'next/image';

type Props = {
  onAnalyze: (file: File) => void;
  isProcessing: boolean;
};

const ImageUploader = ({ onAnalyze, isProcessing }: Props) => {
  const [preview, setPreview] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);

  const onDrop = useCallback((acceptedFiles: File[]) => {
    if (acceptedFiles && acceptedFiles.length > 0) {
      const selectedFile = acceptedFiles[0];
      setFile(selectedFile);
      setPreview(URL.createObjectURL(selectedFile));
    }
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: { 'image/*': ['.jpeg', '.png', '.gif'] }, // .nii gibi formatlar için özelleştirilebilir
    multiple: false,
  });
  
  const handleAnalyzeClick = () => {
    if (file) {
      onAnalyze(file);
    }
  };

  return (
    <div className="bg-secondary/50 rounded-2xl p-6 h-full flex flex-col justify-between border border-border">
      <div>
        <h2 className="text-2xl font-semibold text-text-primary mb-4">Görüntü Yükle</h2>
        <div
          {...getRootProps()}
          className={`border-2 border-dashed border-border rounded-xl p-8 text-center cursor-pointer transition-colors duration-300 ${isDragActive ? 'bg-accent/20 border-accent' : 'hover:bg-primary/50'}`}
        >
          <input {...getInputProps()} />
          {preview ? (
            <div className="relative w-full h-64">
              <Image src={preview} alt="Yüklenen Görüntü Önizlemesi" layout="fill" objectFit="contain" className="rounded-md" />
            </div>
          ) : (
            <div className="flex flex-col items-center text-text-secondary">
              <CloudArrowUp size={48} className="mb-4 text-accent" />
              <p className="font-semibold">Analiz için bir MRI görüntüsü seçin</p>
              <p className="text-sm">Sürükleyip bırakın veya tıklayın</p>
            </div>
          )}
        </div>
      </div>
      <button
        onClick={handleAnalyzeClick}
        disabled={!file || isProcessing}
        className="w-full mt-6 inline-flex items-center justify-center px-6 py-3 text-base font-medium rounded-lg text-white bg-gradient-to-r from-accent-light to-accent hover:opacity-90 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <Brain size={20} className="mr-2" />
        {isProcessing ? 'Analiz Ediliyor...' : 'Analiz Et'}
      </button>
    </div>
  );
};

export default ImageUploader;