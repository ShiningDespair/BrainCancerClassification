using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class Disease
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
}
