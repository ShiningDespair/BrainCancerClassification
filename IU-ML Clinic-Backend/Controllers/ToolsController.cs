using BrainCancerClassification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainCancerClassification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IuMlContext _context;

        public ToolsController(IuMlContext context)
        {
            _context = context;
        }

        // GET: api/Tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetTools()
        {
            return await _context.Tools
                .Include(t => t.ToolImages) 
                .Select(t => new ToolDto  
                {
                    Id = t.Id,
                    Icon = t.Icon,
                    Title = t.Title,
                    Description = t.Description,
                    Href = t.Href,
                    ToolImages = t.ToolImages.Select(ti => new ToolImageDto
                    {
                        Id = ti.Id,
                        Src = ti.Src,
                        Alt = ti.Alt
                    }).ToList()
                }).ToListAsync();
        }
    }


    public class ToolDto
    {
        public int Id { get; set; }
        public string Icon { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Href { get; set; }
        public List<ToolImageDto> ToolImages { get; set; } = new List<ToolImageDto>();
    }

    public class ToolImageDto
    {
        public int Id { get; set; }
        public string Src { get; set; } = null!;
        public string Alt { get; set; } = null!;
    }
}