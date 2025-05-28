namespace HeartDiseaseAPI.Models
{
    public class HeartDiseasePrediction
    {
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}