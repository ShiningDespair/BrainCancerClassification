using OpenCvSharp;
using System.Threading.Tasks;

namespace BrainCancerClassification.Services
{
    public interface IPredictionService
    {
        Task<float[]> PredictAsync(int modelId, Mat image);
    }
}
