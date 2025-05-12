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
    }
}