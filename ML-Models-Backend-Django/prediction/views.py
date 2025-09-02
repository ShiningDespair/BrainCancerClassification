import logging
import tensorflow as tf
import numpy as np
import os
import cv2  # OpenCV for image processing
from PIL import Image
from django.conf import settings
from django.http import JsonResponse
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from rest_framework.parsers import MultiPartParser, FormParser
from .serializers import PredictionSerializer
from .models import Predictionmodels
from tensorflow.keras.applications import resnet50

# --- Setup ---
logger = logging.getLogger(__name__)

# --- Custom Exception Handler ---
def custom_exception_handler(exc, context):
    """
    Custom exception handler that ensures JSON responses for all errors.
    """
    from rest_framework.views import exception_handler
    
    response = exception_handler(exc, context)
    
    if response is not None:
        custom_response_data = {
            'error': 'An error occurred',
            'details': str(exc),
            'status_code': response.status_code
        }
        response.data = custom_response_data
    
    return response

# --- API Views ---

class PredictView(APIView):
    parser_classes = (MultiPartParser, FormParser)
    
    def post(self, request, model_id, *args, **kwargs):
        if not request.FILES.get('image'):
            return Response({"error": "No image file provided"}, status=status.HTTP_400_BAD_REQUEST)
            
        logger.info(f"📨 Received prediction request for model ID: {model_id}")

        try:
            model_data = load_model(model_id)
            if model_data is None:
                return Response({"error": f"Model with ID {model_id} could not be loaded."}, status=status.HTTP_404_NOT_FOUND)

            model = model_data['model']
            labels = model_data['labels']
            img_width = model_data['img_width']
            img_height = model_data['img_height']
            model_record = model_data['model_record']

            logger.info(f"⚙️ Using params: Shape=({img_width},{img_height}), Model='{model_record.modelname}'")

            serializer = PredictionSerializer(data=request.data)
            if not serializer.is_valid():
                return Response({"error": "Invalid request data", "details": serializer.errors}, status=status.HTTP_400_BAD_REQUEST)
            
            image_file = serializer.validated_data['image']
            image = Image.open(image_file)
            image_array = np.array(image.convert('RGB'))

            # ---  Preprocessing Logic ---

            # 🧠 Brain Tumor Model (ResNet50)
            if 'Tumor' in model_record.modelname:
                logger.debug("Applying Brain Tumor specific preprocessing (Grayscale -> Filter -> BONE Colormap -> Resize).")
                gray_image = cv2.cvtColor(image_array, cv2.COLOR_RGB2GRAY)
                filtered_image = cv2.bilateralFilter(gray_image, 2, 50, 50)
                bone_image = cv2.applyColorMap(filtered_image, cv2.COLORMAP_BONE)
                resized_image = cv2.resize(bone_image, (img_width, img_height))
                final_image_array = resized_image / 255.0
                final_image_array = np.expand_dims(final_image_array, axis=0)

            # 🧠 Alzheimer's Model (BaselineCNN)
            elif 'Alzheimer' in model_record.modelname:
                logger.debug("Applying Alzheimer specific preprocessing (Grayscale -> Resize).")
                gray_image = cv2.cvtColor(image_array, cv2.COLOR_RGB2GRAY)
                resized_image = cv2.resize(gray_image, (img_width, img_height))
                resized_image_with_channel = np.expand_dims(resized_image, axis=-1)
                final_image_array = resized_image_with_channel / 255.0
                final_image_array = np.expand_dims(final_image_array, axis=0)
            
            # Fallback for other models (e.g., MS, Lung Cancer)
            else:
                logger.debug("Applying simple RGB preprocessing (default).")
                resized_image = cv2.resize(image_array, (img_width, img_height))
                final_image_array = resized_image / 255.0
                final_image_array = np.expand_dims(final_image_array, axis=0)

            # --- End of Preprocessing ---

            logger.debug(f"🔍 Final preprocessed shape for model: {final_image_array.shape}")
            logger.info("🔮 Making prediction...")
            predictions = model.predict(final_image_array)
            
            # Regression vs. Classification Logic
            if model_record.numclasses == 1:
                predicted_value = predictions[0][0]
                logger.info(f"✅ Prediction: {labels[0]} value is {predicted_value:.4f}")
                result_data = {"predictedLabel": labels[0], "predictedValue": float(predicted_value)}
            else:
                scores = predictions[0]
                predicted_index = np.argmax(scores)
                predicted_label = labels[predicted_index]
                confidence = np.max(scores)
                logger.info(f"✅ Prediction: {predicted_label} (confidence: {confidence:.4f})")
                result_data = {
                    "predictedLabel": predicted_label,
                    "confidence": float(confidence),
                    "scores": scores.tolist()
                }
            
            return Response(result_data, status=status.HTTP_200_OK)

        except Exception as e:
            logger.error(f"🔴 Prediction error: {e}", exc_info=True)
            return Response({"error": "An internal error occurred during processing.", "details": str(e)}, status=status.HTTP_500_INTERNAL_SERVER_ERROR)


class HealthCheckView(APIView):
    """
    A simple view to check if the server is running.
    """
    def get(self, request, *args, **kwargs):
        return Response({"status": "ok", "message": "Server is healthy."}, status=status.HTTP_200_OK)


# --- Model Loading and Caching ---
MODEL_CACHE = {}

def load_model(model_id):
    if model_id in MODEL_CACHE:
        logger.info(f"✅ Model with ID {model_id} found in cache.")
        return MODEL_CACHE[model_id]

    try:
        model_record = Predictionmodels.objects.get(id=model_id)
        
        labels_str = model_record.labels
        labels = [label.strip() for label in labels_str.split(',')] if labels_str else []
        
        input_shape_str = model_record.inputshape
        if input_shape_str:
            input_dims = [int(dim) for dim in input_shape_str.split(',')]
            img_width, img_height, channels = input_dims[0], input_dims[1], input_dims[2]
        else: # Default values if not specified in database
            img_width, img_height, channels = 224, 224, 3

        model_path = os.path.join(settings.BASE_DIR, model_record.filepath)
        model_path = os.path.normpath(model_path)

        if not os.path.exists(model_path):
            logger.error(f"🔴 CRITICAL: Model file not found at {model_path}!")
            return None

        logger.info(f"🔄 Loading model with ID {model_id} from {model_path}...")
        loaded_model = tf.keras.models.load_model(model_path, compile=False)
        logger.info(f"✅ Model with ID {model_id} loaded successfully!")
        
        model_data = {
            'model': loaded_model,
            'labels': labels,
            'img_width': img_width,
            'img_height': img_height,
            'channels': channels,
            'model_record': model_record 
        }
        
        MODEL_CACHE[model_id] = model_data
        return model_data

    except Predictionmodels.DoesNotExist:
        logger.error(f"🔴 Model with ID {model_id} does not exist in the database.")
        return None
    except Exception as e:
        logger.error(f"🔴 Failed to load model with ID {model_id}. Error: {e}", exc_info=True)
        return None