using AutoMapper;
using Appointment.Data.Entities;
using Appointment.DTOs;

namespace Appointment.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();

        }
    }
}
