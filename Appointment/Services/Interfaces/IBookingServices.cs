using Appointment.Data.Entities;
using Appointment.DTOs;
using Appointment.Model;
using Common.Results;

namespace Appointment.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResult<BookingDto>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<BookingDto>>> GetAllAsync();
        Task<ServiceResult<BookingDto>> AddAsync(BookingDto booking);
        Task<ServiceResult<BookingDto>> UpdateAsync(int id, BookingDto booking);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<BookingDto>>> GetFilteredBookingsAsync(BookingFilter filter);
        Task<ServiceResult<IEnumerable<DateTimeOffset>>> GetFreeTimeAsync(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate, int vetAidId);
        Task<ServiceResult<bool>> SetStatusAsync(int id, AppointmentStatus status);
    }
}
