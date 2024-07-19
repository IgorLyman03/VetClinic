using AutoMapper;
using DoctorProfile.Data.Entities;
using DoctorProfile.DTOs;
using DoctorProfile.Model;

namespace DoctorProfile.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DoctorInfo, DoctorInfoDto>().ReverseMap();
            CreateMap<DoctorTimetable, DoctorTimetableDto>().ReverseMap();
            CreateMap<DoctorTimetableSegment, DoctorTimetableSegmentDto>().ReverseMap();
        }
    }
}
