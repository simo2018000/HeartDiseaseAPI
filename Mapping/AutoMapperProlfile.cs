using AutoMapper;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;

namespace HeartDiseaseAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // For reading patient data (excluding password, etc.)
            CreateMap<Patient, PatientReadDto>();
            CreateMap<PatientCreateDto, Patient>(); // Updated from PatientCreateDtos
        }
      
    }
}