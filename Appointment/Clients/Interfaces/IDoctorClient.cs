using Appointment.DTOs;
using Common.Results;

namespace Appointment.Clients.Interfaces
{
    public interface IDoctorClient
    {
        Task<ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>> GetTimetableByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
