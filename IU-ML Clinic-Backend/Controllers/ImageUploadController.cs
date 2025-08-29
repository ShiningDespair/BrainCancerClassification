// Namespacelerinizin doğru olduğundan emin olun
using BrainCancerClassification.Models;
using BrainCancerClassification.Helpers;
using BrainCancerClassification.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Gerekli olabilir
using System;
using System.Threading.Tasks;

namespace BrainCancerClassification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        private readonly IuMlContext _context;

        public ImageUploadController(IPredictionService predictionService, IuMlContext context)
        {
            _predictionService = predictionService;
            _context = context;
        }

        [HttpPost("{modelId}")]
        public async Task<IActionResult> UploadImage(int modelId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            try
            {
                var modelInfo = await _context.PredictionModels.FindAsync(modelId);
                if (modelInfo == null)
                {
                    return NotFound(new { message = "Model not found." });
                }

                var shapeParts = modelInfo.InputShape.Split(',');
                if (shapeParts.Length < 2)
                {
                    return StatusCode(500, new { message = "Invalid InputShape format in database." });
                }
                int height = int.Parse(shapeParts[0].Trim());
                int width = int.Parse(shapeParts[1].Trim());

                using (var stream = file.OpenReadStream())
                {
                    var preprocessedImage = ImagePreprocessingHelper.PreprocessImage(stream, width, height);

                    var prediction = await _predictionService.PredictAsync(modelId, preprocessedImage);

                    return Ok(new { prediction });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during prediction for model {modelId}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during image processing or prediction." });
            }
        }
    }
}