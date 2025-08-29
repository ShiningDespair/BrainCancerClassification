namespace BrainCancerClassification.DTOs
{
    public class ImageProcessingOptions
    {
        // Common spaces for all
        public int ImgSize { get; set; }
        public string TrainingDir { get; set; } = string.Empty;
        public string TestingDir { get; set; } = string.Empty;
        public string? CleanedTrainingDir { get; set; } = string.Empty;
        public string? CleanedTestingDir { get; set; } = string.Empty;
        public string? OutputDir { get; set; } = string.Empty;
        public string? BaseDir { get; set; } 
    }
}
