namespace HeartDiseaseAPI.Dtos
{
    public class PatientCreateDtos
    {

        public int Age { get; set; } // in days
        public String Sex { get; set; } = String.Empty; //H OR F  
        public int Height { get; set; } // in cm
        public int Weight { get; set; } // in kg
        public float BloodPressureLow { get; set; } // in mmHg
        public float BloodPressureHigh { get; set; } // in mmHg
        public float Cholesterol { get; set; }
        public float Glucose { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }

    }
}