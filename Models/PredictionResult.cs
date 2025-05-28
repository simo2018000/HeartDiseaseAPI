namespace HeartDiseaseAPI.Models // Or any other appropriate namespace
{
    public class PredictionResult
    {
        public bool HasHeartDisease { get; set; }
        public float Confidence { get; set; }
        // You can add other properties if needed, like a Score or specific probabilities
    }
}