using AutoMapper;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;

namespace HeartDiseaseAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Patient, PatientReadDto>(); // This should be sufficient by convention
            CreateMap<PatientCreateDto, Patient>();
        }
    }
}