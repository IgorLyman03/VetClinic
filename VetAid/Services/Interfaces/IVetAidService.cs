using Common.Results;

namespace VetAid.Services.Interfaces
{
    public interface IVetAidService
    {
        Task<ServiceResult<IEnumerable<VetAidDto>>> GetAllAsync();
        Task<ServiceResult<VetAidDto>> GetByIdAsync(int id);
        Task<ServiceResult<VetAidDto>> AddAsync(VetAidDto dto);
        Task<ServiceResult<VetAidDto>> UpdateAsync(VetAidDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
