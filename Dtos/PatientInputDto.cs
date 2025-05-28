using System.ComponentModel.DataAnnotations;

namespace HeartDiseaseAPI.Dtos
{
    public class PatientInputDto
    {
        [Required]
        public int Age { get; set; }

        [Required]
        [RegularExpression("^(F|f|M|m)$")]
        public string Sex { get; set; } = "M";

        [Required]
        [Range(0, 300, ErrorMessage = "Taille entre 0 et 300 cm")]
        public int Height { get; set; }

        [Required]
        [Range(30, 300, ErrorMessage ="poids entre 30 et 300 kg")]
        public int Weight { get; set; }

        [Required]
        [Range(30, 250, ErrorMessage = " tension ater")]
        public float BloodPressureLow { get; set; }

        [Required]
        [Range(50, 300)]
        public float BloodPressureHigh { get; set; }

        [Required]
        [Range(50, 400)]
        public float Cholesterol { get; set; }

        [Required]
        [Range(30, 600)]
        public float Glucose { get; set; }

        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
    }
}