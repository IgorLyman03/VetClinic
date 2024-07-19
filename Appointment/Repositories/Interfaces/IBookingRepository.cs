using Appointment.Data.Entities;
using Common.Results;

namespace Appointment.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<ServiceResult<Booking>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<Booking>>> GetAllAsync();
        Task<ServiceResult<Booking>> AddAsync(Booking booking);
        Task<ServiceResult<Booking>> UpdateAsync(int id, Booking booking);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<Booking>>> GetFilteredBookingsAsync(string? clientEmail, string? doctorId, int? vetAidId, DateTimeOffset? startDate, DateTimeOffset? endDate);
        Task<ServiceResult<bool>> IsFreeToBooking(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
