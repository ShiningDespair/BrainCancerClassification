// Namespacelerinizin doğru olduğundan emin olun
using BrainCancerClassification.DTOs;
using BrainCancerClassification.Helpers;
using BrainCancerClassification.Models;
using BrainCancerClassification.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                return BadRequest(new { message = "Yüklenecek dosya bulunamadı." });
            }

            try
            {
                var modelInfo = await _context.PredictionModels.FindAsync(modelId);
                if (modelInfo == null)
                {
                    return NotFound(new { message = $"ID'si {modelId} olan model bulunamadı." });
                }

                if (string.IsNullOrWhiteSpace(modelInfo.InputShape))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Modelin giriş şekli (InputShape) veritabanında tanımlanmamış." });
                }

                var shapeParts = modelInfo.InputShape.Split(',');
                if (shapeParts.Length < 2 || !int.TryParse(shapeParts[0].Trim(), out int height) || !int.TryParse(shapeParts[1].Trim(), out int width))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Modelin giriş şekli (InputShape) veritabanında geçersiz formatta." });
                }

                using (var stream = file.OpenReadStream())
                {
                    var preprocessedImage = ImagePreprocessingHelper.PreprocessImage(stream, width, height);
                    var predictionResult = await _predictionService.PredictAsync(modelId, preprocessedImage);
                    return Ok(predictionResult);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Geçersiz dosya formatı veya bozuk resim dosyası.", error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during prediction for model {modelId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Görüntü işleme veya tahmin sırasında beklenmeyen bir hata oluştu." });
            }
        }
    }
}