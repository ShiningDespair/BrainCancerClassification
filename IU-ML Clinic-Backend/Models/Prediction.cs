using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class Prediction
{
    public int Id { get; set; }

    public int UploadId { get; set; }

    public int ModelId { get; set; }

    public int? DiseaseId { get; set; }

    public string PredictionResult { get; set; } = null!;

    public decimal? Confidence { get; set; }

    public DateTime PredictedAt { get; set; }

    public virtual Disease? Disease { get; set; }

    public virtual PredictionModel Model { get; set; } = null!;

    public virtual Upload Upload { get; set; } = null!;
}
