using BrainCancerClassification.DTOs;
using OpenCvSharp;
using System.Threading.Tasks;

namespace BrainCancerClassification.Services
{
    public interface IPredictionService
    {
        Task<PredictionResultDto> PredictAsync(int modelId, Mat image);
    }
}
