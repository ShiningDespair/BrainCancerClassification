import { ToolType, PredictionModelType } from '@/types/Types';
import { PredictionResult, ClassificationResult, RegressionResult } from '@/types/Types';

// Define the type for the upload response
export interface UploadResult {
    message: string;
    filePath?: string;
}

const API_BASE_URL_DJANGO = 'http://127.0.0.1:8080/api';
const API_BASE_URL_DOTNET = 'https://localhost:7280/api';

// Helper function to handle API responses
const handleResponse = async (response: Response) => {
    const contentType = response.headers.get('content-type');
    
    if (!response.ok) {
        let errorMessage = 'Network request failed';
        
        if (contentType?.includes('application/json')) {
            try {
                const errorData = await response.json();
                errorMessage = errorData.error || errorData.message || `HTTP ${response.status}: ${response.statusText}`;
            } catch (jsonError) {
                errorMessage = `HTTP ${response.status}: ${response.statusText}`;
            }
        } else if (contentType?.includes('text/html')) {
            // This is the HTML error page issue
            errorMessage = `Server returned HTML error page. Check if Django server is running on ${API_BASE_URL_DJANGO}`;
            console.error('Received HTML instead of JSON. This usually means:');
            console.error('1. Django server is not running');
            console.error('2. URL endpoint is incorrect');
            console.error('3. CORS issue');
            console.error('4. Server error causing Django to return error page');
        } else {
            errorMessage = `HTTP ${response.status}: ${response.statusText}`;
        }
        
        throw new Error(errorMessage);
    }
    
    if (!contentType?.includes('application/json')) {
        throw new Error('Server did not return JSON response');
    }
    
    return response.json();
};

export const getTools = async (): Promise<ToolType[]> => {
    try {
        const response = await fetch(`${API_BASE_URL_DOTNET}/tools`);
        return await handleResponse(response);
    } catch (error) {
        console.error('Error fetching tools:', error);
        throw error;
    }
};

export const uploadImage = async (modelId: string, file: File): Promise<UploadResult> => {
    try {
        const formData = new FormData();
        formData.append('file', file);

        const response = await fetch(`${API_BASE_URL_DOTNET}/imageupload/${modelId}`, {
            method: 'POST',
            body: formData,
        });

        return await handleResponse(response);
    } catch (error) {
        console.error('Error uploading image:', error);
        throw error;
    }
};

export const getPredictionModel = async (modelId: string): Promise<PredictionModelType> => {
    try {
        const response = await fetch(`${API_BASE_URL_DOTNET}/predictionmodels/${modelId}/`);
        return await handleResponse(response);
    } catch (error) {
        console.error('Error fetching prediction model:', error);
        throw error;
    }
};

export const predictImage = async (modelId: string, file: File): Promise<PredictionResult> => {
    try {
        // First check if Django server is reachable
        const healthCheck = await fetch(`${API_BASE_URL_DJANGO}/health/`, { 
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            }
        }).catch(() => null);
        
        if (!healthCheck) {
            throw new Error(`Django server is not reachable at ${API_BASE_URL_DJANGO}. Make sure it's running.`);
        }

        const formData = new FormData();
        formData.append('image', file);

        const response = await fetch(`${API_BASE_URL_DJANGO}/predict/${modelId}/`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
            },
            body: formData,
        });

        return await handleResponse(response);
    } catch (error) {
        console.error('Error predicting image:', error);
        throw error;
    }
};