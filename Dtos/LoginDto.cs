using System.ComponentModel.DataAnnotations;
using HeartDiseaseAPI.Models;
namespace HeartDiseaseAPI.Dtos


{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email invalide")]
        [StringLength(100, ErrorMessage = "Email ne doit pas dépasser 100 caractères")]
        public string Email { get; set; } = String.Empty;
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mot de passe doit contenir entre 6 et 100 caractères")]
        public string Password { get; set; } = String.Empty;


    }
    
}
