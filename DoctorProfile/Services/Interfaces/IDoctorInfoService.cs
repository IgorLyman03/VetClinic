using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data.Entities;
using DoctorProfile.DTOs;

namespace DoctorProfile.Services.Interfaces
{
    public interface IDoctorInfoService
    {
        Task<ServiceResult<IEnumerable<DoctorInfoDto>>> GetAllAsync();
        Task<ServiceResult<DoctorInfoDto>> GetByUserIdAsync(string userId);
        Task<ServiceResult<DoctorInfoDto>> GetByIdAsync(int id);
        Task<ServiceResult<DoctorInfoDto>> AddAsync(DoctorInfoDto profile);
        Task<ServiceResult<DoctorInfoDto>> UpdateAsync(int id, DoctorInfoDto profile);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
