using AutoMapper;
using HeartDiseaseAPI.Dtos;


namespace HeartDiseaseAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<HeartDiseaseAPI.Models, HeartDiseaseAPI.Models>();
            CreateMap<PatientCreateDto, Patient>();
        }
    }
}
