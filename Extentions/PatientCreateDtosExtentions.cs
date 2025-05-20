using HeartDiseaseAPI.Models;
using HeartDiseaseAPI.Dtos;

namespace HeartDiseaseAPI.Extentions
{
    public static class PatientCreateDtosExtensions
    {
        public static Patient ToPatient(this PatientCreateDtos dto)
        {
            return new Patient
            {
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
