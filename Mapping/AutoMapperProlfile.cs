using AutoMapper;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;

namespace HeartDiseaseAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Patient, PatientReadDtos>();
            CreateMap<PatientCreateDtos, Patient>();
        }
        private int NormalizeSex(string Sex)
        {
            Sex = Sex.Trim().ToUpper();

            if (Sex == "H" || Sex == "M")
                return 1;
            else if (Sex == "F")
                return 0;
            else
                throw new ArgumentException($"Invalid sex value: {Sex}. Expected 'M', 'F', or 'H'.");
        }

    }
}
