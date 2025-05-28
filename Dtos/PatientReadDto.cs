namespace HeartDiseaseAPI.Dtos
{
    public class PatientReadDto // Renamed from PatientReadDtos
    {
        public PatientReadDto() { }
        public string? Id { get; set; } // Changed from int ID, made nullable string to match Patient model
        public string LastName { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public int Age { get; set; } // in years
        public String Sex { get; set; } = string.Empty;
        public int Height { get; set; } // in cm
        public int Weight { get; set; } // in kg
        public float BloodPressureLow { get; set; } // in mmHg
        public float BloodPressureHigh { get; set; } // in mmHg
        public float Cholesterol { get; set; } // in mg/dL
        public float Glucose { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
        public bool HasHeartDisease { get; set; } // It might be useful to see this on read
    }
}