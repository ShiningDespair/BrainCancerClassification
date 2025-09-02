using BrainCancerClassification.DTOs;
using BrainCancerClassification.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrainCancerClassification.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IuMlContext _context;

        public PredictionService(IuMlContext context)
        {
            _context = context;
        }

        public async Task<PredictionResultDto> PredictAsync(int modelId, Mat image)
        {
            var model = await _context.PredictionModels.FindAsync(modelId);
            if (model == null || string.IsNullOrEmpty(model.FilePath))
            {
                throw new Exception("Model not found or file path is not specified.");
            }

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullModelPath = Path.Combine(basePath, model.FilePath);

            if (!File.Exists(fullModelPath))
            {
                throw new FileNotFoundException($"Model file not found at: {fullModelPath}");
            }

            using var session = new InferenceSession(fullModelPath);

            var inputName = session.InputMetadata.Keys.First();
            var outputName = session.OutputMetadata.Keys.First();

            var inputTensor = MatToTensor(image);

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
            };

            using var results = session.Run(inputs);

            var outputTensor = results.First().AsTensor<float>();
            var predictionArray = outputTensor.ToArray();

            var maxScore = predictionArray.Max();
            var maxIndex = predictionArray.ToList().IndexOf(maxScore);
            var labels = model.Labels?.Split(',') ?? new string[] { "Unknown" };
            var predictedLabel = labels.Length > maxIndex ? labels[maxIndex].Trim() : "Error: Label index out of range.";

            return new PredictionResultDto
            {
                PredictedLabel = predictedLabel,
                Confidence = maxScore,
                Scores = predictionArray
            };
        }

        private DenseTensor<float> MatToTensor(Mat image)
        {
            var height = image.Height;
            var width = image.Width;
            var channels = image.Channels();

            var dimensions = new[] { 1, height, width, channels };
            var floatArray = new float[height * width * channels];

            var imageBytes = image.ToBytes();
            for (int i = 0; i < imageBytes.Length; i++)
            {
                floatArray[i] = imageBytes[i] / 255.0f;
            }

            return new DenseTensor<float>(floatArray, dimensions);
        }
    }
}