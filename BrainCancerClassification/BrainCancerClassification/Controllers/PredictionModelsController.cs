using BrainCancerClassification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BrainCancerClassification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionModelsController : ControllerBase
    {
        private readonly IuMlContext _context;

        public PredictionModelsController(IuMlContext context)
        {
            _context = context;
        }

        // GET: api/PredictionModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PredictionModel>> GetPredictionModel(int id)
        {
            var predictionModel = await _context.PredictionModels.FindAsync(id);

            if (predictionModel == null)
            {
                return NotFound();
            }

            return predictionModel;
        }
    }
}
