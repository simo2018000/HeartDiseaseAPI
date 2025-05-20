using System.ComponentModel.DataAnnotations;
namespace HeartDiseaseAPI.Dtos
{
    public class UpdatedPatientDto
    {
        [Required(ErrorMessage = "Age est obligatoire")]
        [Range(0, 120, ErrorMessage = "Age doit être entre 0 et 120")]
        public int Age { get; set; } // in years 
        [Required(ErrorMessage = "Sexe est obligatoire")]
        [RegularExpression("^(H|h|F|f|M|m)$", ErrorMessage = "Sex doit etre H,h,F,f,M,m.")]
        public String Sex { get; set; } = String.Empty; //H OR F  
        [Required(ErrorMessage = "Taille est obligatoire")]
        [Range(20,500, ErrorMessage = "Taille doit être entre 20 et 500 cm")]
        public int Height { get; set; } // in cm
        [Required(ErrorMessage = "Poids est obligatoire")]
        [Range(1, 500, ErrorMessage = "Poids doit être entre 1 et 500 Kg")]
        public int Weight { get; set; } // in kg
        [Required(ErrorMessage = "Tension artérielle basse est obligatoire")]
        [Range(30, 250, ErrorMessage = "Tension artérielle basse doit être entre 30 et 250 mmHg")]
        public float BloodPressureLow { get; set; } // in mmHg
        [Required(ErrorMessage = "Tension artérielle haute est obligatoire")]
        [Range(50, 300, ErrorMessage = "Tension artérielle haute doit être entre 50 et 300 mmHg")]
        public float BloodPressureHigh { get; set; } // in mmHg
        [Required(ErrorMessage = "Cholestérol est obligatoire")]
        [Range(50, 400, ErrorMessage = "Cholestérol doit être entre 50 et 400 mg/dL")]
        public float Cholesterol { get; set; }
        [Required(ErrorMessage = "Glucose est obligatoire")]
        [Range(30, 600, ErrorMessage = "Glucose doit être entre 30 et 600 mg/dL")]
        public float Glucose { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
    }
}
