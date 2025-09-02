namespace BrainCancerClassification.DTOs
{
    public class PredictionResultDto
    {
        public string PredictedLabel { get; set; }
        public float Confidence { get; set; }
        public float[] Scores { get; set; }
    }
}
