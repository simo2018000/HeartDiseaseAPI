using AutoMapper;
using HeartDiseaseAPI.Dtos;


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
