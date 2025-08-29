import { ToolType, PredictionModelType, PredictionResultType } from '@/types/Types';

const API_BASE_URL = 'https://localhost:7280/api'; // Backend URL'nize göre ayarlayın

export const getTools = async (): Promise<ToolType[]> => {
  const response = await fetch(`${API_BASE_URL}/tools`);
  if (!response.ok) {
    throw new Error('Failed to fetch tools');
  }
  return response.json();
};

// Değişiklik burada yapıldı: Promise<any> -> Promise<PredictionResultType>
export const uploadImage = async (modelId: string, file: File): Promise<PredictionResultType> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await fetch(`${API_BASE_URL}/imageupload/${modelId}`, {
    method: 'POST',
    body: formData,
  });

  if (!response.ok) {
    throw new Error('Failed to upload image');
  }

  return response.json();
};

export const getPredictionModel = async (modelId: string): Promise<PredictionModelType> => {
  const response = await fetch(`${API_BASE_URL}/predictionmodels/${modelId}`);
  if (!response.ok) {
    throw new Error('Failed to fetch prediction model');
  }
  return response.json();
};