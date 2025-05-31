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
    }
}