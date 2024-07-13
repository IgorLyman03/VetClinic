using Common.Results;
using DoctorProfile.Data.Entities;

namespace DoctorProfile.Repositories.Interfaces
{
    public interface IDoctorInfoRepository
    {
        Task<ServiceResult<IEnumerable<DoctorInfo>>> GetAllAsync();
        Task<ServiceResult<DoctorInfo>> GetByUserIdAsync(string userId);
        Task<ServiceResult<DoctorInfo>> GetByIdAsync(int id);
        Task<ServiceResult<DoctorInfo>> AddAsync(DoctorInfo profile);
        Task<ServiceResult<DoctorInfo>> UpdateAsync(int id, DoctorInfo profile);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
