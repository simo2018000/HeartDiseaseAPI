using System.Globalization;

namespace HeartDiseaseAPI.Models
{
    public class Patient
    {
        //IDENTIFICATION DATA
        public string LastName { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        //HEALTH DATA
        public int ID { get; set; }
        public int Age { get; set; }// in days
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
        public bool HasHeartDisease { get; set; }

        internal static object FindIndex()
        {
            throw new NotImplementedException();
        }
    }
}
