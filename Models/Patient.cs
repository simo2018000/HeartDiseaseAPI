using System.Globalization;

namespace HeartDiseaseAPI.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public int Age { get; set; }// in days
        public String Sex { get; set; } = string.Empty;
        public int Height { get; set; } // in cm
        public int Weight { get; set; } // in kg
        public float BloodPressureLow { get; set; } // in mmHg
        public float BloodPressureHigh { get; set; } // in mmHg
        public float Cholesterol { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
        public bool HasHeartDisease { get; set; }

    }
}
