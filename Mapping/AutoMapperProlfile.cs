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
        private int NormalizeSex(String Sex){
            //input flexible exemple {"m","f","H","M","h","m"}

            Sex = Sex.Trim().ToUpper();
            return Sex == "H" || Sex == "M" ? 1 : 0;


        }
    }
}