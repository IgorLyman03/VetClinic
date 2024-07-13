using AutoMapper;
using DoctorProfile.Data.Entities;
using DoctorProfile.DTOs;

namespace DoctorProfile.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DoctorInfo, DoctorInfoDto>().ReverseMap();
            CreateMap<DoctorTimetable, DoctorTimetableDto>().ReverseMap();
        }
    }
}
