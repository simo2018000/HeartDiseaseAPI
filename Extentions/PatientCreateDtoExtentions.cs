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
                Age = dto.Age,
                Sex = dto.Sex,
                Height = dto.Height,
                Weight = dto.Weight,
                BloodPressureLow = dto.BloodPressureLow,
                BloodPressureHigh = dto.BloodPressureHigh,
                Cholesterol = dto.Cholesterol,
                Glucose = dto.Glucose,
                IsSmoker = dto.IsSmoker,
                IsAlcoholic = dto.IsAlcoholic,
                IsActive = dto.IsActive
            };
        }
    }
}
