using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class ToolImage
{
    public int Id { get; set; }

    public int ToolId { get; set; }

    public string Src { get; set; } = null!;

    public string Alt { get; set; } = null!;

    public virtual Tool Tool { get; set; } = null!;
}
