using AutoMapper;
using HeartDiseaseAPI.Dtos;
using HeartDiseaseAPI.Models;
using static HeartDiseaseAPI.Dtos.PatientDtos;

namespace HeartDiseaseAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Patient, PatientReadDto>();
            CreateMap<PatientCreateDto, Patient>();
        }
    }
}