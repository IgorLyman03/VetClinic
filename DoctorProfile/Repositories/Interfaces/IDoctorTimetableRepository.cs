using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data.Entities;

namespace DoctorProfile.Repositories.Interfaces
{
    public interface IDoctorTimetableRepository
    {
        Task<ServiceResult<IEnumerable<DoctorTimetable>>> GetAllAsync();
        Task<ServiceResult<DoctorTimetable>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<DoctorTimetable>>> GetByUserIdAsync(string profileId);
        Task<ServiceResult<IEnumerable<DoctorTimetable>>> GetByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<ServiceResult<DoctorTimetable>> AddAsync(DoctorTimetable availability);
        Task<ServiceResult<DoctorTimetable>> UpdateAsync(int id, DoctorTimetable availability);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
