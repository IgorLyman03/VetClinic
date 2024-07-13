using Common.Results;

namespace VetAid.Repositories.Interfaces
{
    public interface IVetAidRepository
    {
        Task<ServiceResult<IEnumerable<VetAidEntity>>> GetAllAsync();
        Task<ServiceResult<VetAidEntity>> GetByIdAsync(int id);
        Task<ServiceResult<VetAidEntity>> AddAsync(VetAidEntity entity);
        Task<ServiceResult<VetAidEntity>> UpdateAsync(VetAidEntity entity);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
