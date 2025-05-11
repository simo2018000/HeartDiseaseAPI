namespace HeartDiseaseAPI.contracts
{
    public class Patient
    {
        public int ID { get; set; }
        public int Age { get; set; } // in days 
        public string Sex { get; set; } = string.Empty; // M or F
        public int Height { get; set; } // in cm
        public int Weight { get; set; } // in kg    
        public float BloodPressureLow { get; set; }
        public float BloodPressureHigh { get; set; }
        public float Cholesterol { get; set; }
        public float Glucose { get; set; }
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
        public bool HasHeartDisease { get; set; }

    }

}