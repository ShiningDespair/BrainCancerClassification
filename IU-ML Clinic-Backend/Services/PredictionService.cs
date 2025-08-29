using BrainCancerClassification.Models;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.Keras.Engine;
using static Tensorflow.Binding;

namespace BrainCancerClassification.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IuMlContext _context;

        public PredictionService(IuMlContext context)
        {
            _context = context;
        }

        public async Task<float[]> PredictAsync(int modelId, Mat image)
        {
            var model = await _context.PredictionModels.FindAsync(modelId);
            if (model == null || string.IsNullOrEmpty(model.FilePath))
            {
                throw new Exception("Model not found or file path is not specified.");
            }

            // Load the model
            var loadedModel = tf.keras.models.load_model(model.FilePath);

            // Convert Mat to Tensor
            var tensor = MatToTensor(image);

            // Make prediction
            var prediction = loadedModel.predict(tensor);
            var predictionArray = prediction.ToArray<float>();

            return predictionArray;
        }

        private Tensor MatToTensor(Mat image)
        {
            var imageBytes = image.ToBytes();
            var shape = new long[] { 1, image.Height, image.Width, image.CvtColor(ColorConversionCodes.BGR2GRAY).Channels() };
            var tensor = tf.constant(imageBytes, dtype: tf.int8);
            tensor = tf.image.decode_image(tensor, channels: 3);
            tensor = tf.expand_dims(tensor, 0); // Add batch dimension
            return tensor;
        }
    }
}
