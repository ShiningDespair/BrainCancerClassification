using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class PredictionModel
{
    public int Id { get; set; }

    public string ModelName { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string? Description { get; set; }

    public string? FilePath { get; set; }

    public string Framework { get; set; } = null!;

    public string? InputShape { get; set; }

    public int NumClasses { get; set; }

    public string? Labels { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
}
