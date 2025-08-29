using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class Tool
{
    public int Id { get; set; }

    public string Icon { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Href { get; set; }

    public virtual ICollection<ToolImage> ToolImages { get; set; } = new List<ToolImage>();
}
