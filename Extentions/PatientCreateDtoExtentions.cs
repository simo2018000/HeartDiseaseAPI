using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Dtos;

namespace HeartDiseaseAPI.Extentions
{
    public static class PatientCreateDtoExtensions
    {
        public static Patient ToPatient(this PatientCreateDto dto)
        {
            return new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                // Password will be hashed in PatientServices.Register, not directly mapped here

                // Set default values for health data since they are no longer part of registration payload
                Age = 0, // Default value or consider making nullable in Patient model
                Sex = "Unknown", // Default value
                Height = 0, // Default value
                Weight = 0, // Default value
                BloodPressureLow = 0.0f, // Default value
                BloodPressureHigh = 0.0f, // Default value
                Cholesterol = 0.0f, // Default value
                Glucose = 0.0f, // Default value
                IsSmoker = false,
                IsAlcoholic = false,
                IsActive = false
            };
        }
    }
}