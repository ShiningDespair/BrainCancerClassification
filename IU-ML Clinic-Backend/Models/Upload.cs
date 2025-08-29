using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class Upload
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FilePath { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();

    public virtual User User { get; set; } = null!;
}
