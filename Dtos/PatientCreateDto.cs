using System.ComponentModel.DataAnnotations;

namespace HeartDiseaseAPI.Dtos
{
    public class PatientCreateDto
    {

        //PATIENT DATA
        [Required(ErrorMessage = "Nom obligatoire")]
        [StringLength(50, ErrorMessage = "Nom ne doit pas dépasser 50 caractères")]
        public string LastName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Prénom obligatoire")]
        [StringLength(50, ErrorMessage = "Prénom ne doit pas dépasser 50 caractères")]
        public string FirstName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Email obligatoire")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        [StringLength(100, ErrorMessage = "Email ne doit pas dépasser 100 caractères")]
        public string Email { get; set; } = String.Empty;
        [Required(ErrorMessage = "Mot de passe obligatoire")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mot de passe doit contenir entre 6 et 100 caractères")]
        public string Password { get; set; } = String.Empty;
        //HEALTH DATA
        [Required(ErrorMessage = "Age obligatoire")]
        [Range(0, 120, ErrorMessage = "Age doit être entre 0 et 120")]
        public int Age { get; set; } // in years
        [Required(ErrorMessage = "Sexe obligatoire")]
        [RegularExpression("^(H|h|F|f|M|m)$",ErrorMessage ="Sex doit etre H,h,F,f,M,m.")]
        public String Sex { get; set; } = String.Empty; //H OR F  
        [Required(ErrorMessage = "Taille obligatoire")]
        [Range(0, 300, ErrorMessage = "Taille doit être entre 20 et 300 cm")]
        public int Height { get; set; } // in cm
        [Required(ErrorMessage = "Poids obligatoire")]
        [Range(0, 500, ErrorMessage = "Poids doit être entre 1 et 500 Kg")]
        public int Weight { get; set; } // in kg
        [Required(ErrorMessage = "Tension artérielle basse obligatoire")]
        [Range(0, 300, ErrorMessage = "Tension artérielle basse doit être entre 30 et 250 mmHg")]
        public float BloodPressureLow { get; set; } // in mmHg
        [Required(ErrorMessage = "Tension artérielle haute obligatoire")]
        [Range(0, 300, ErrorMessage = "Tension artérielle haute doit être entre 50 et 300 mmHg")]
        public float BloodPressureHigh { get; set; } // in mmHg
        [Required(ErrorMessage = "Cholestérol obligatoire")]
        [Range(0, 500, ErrorMessage = "Cholestérol doit être entre 50 et 400 mg/dL")]
        public float Cholesterol { get; set; }
        [Required(ErrorMessage = "Glucose obligatoire")]
        [Range(0, 500, ErrorMessage = "Glucose doit être entre 30 et 600 mg/dL")]
        public float Glucose { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }

    }
}