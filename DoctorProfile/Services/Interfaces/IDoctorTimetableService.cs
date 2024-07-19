using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.DTOs;

namespace DoctorProfile.Services.Interfaces
{
    public interface IDoctorTimetableService
    {
        Task<ServiceResult<IEnumerable<DoctorTimetableDto>>> GetAllAsync();
        Task<ServiceResult<DoctorTimetableDto>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<DoctorTimetableDto>>> GetByUserIdAsync(string userId);
        Task<ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>> GetSegmentsByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<ServiceResult<DoctorTimetableDto>> AddAsync(DoctorTimetableDto availabilityDto);
        Task<ServiceResult<DoctorTimetableDto>> UpdateAsync(int id, DoctorTimetableDto availabilityDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
