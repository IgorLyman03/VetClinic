using Common.Results;

namespace VetAid.Repositories.Interfaces
{
    public interface IAnimalTypeRepository
    {
        Task<ServiceResult<IEnumerable<AnimalTypeEntity>>> GetAllAsync();
        Task<ServiceResult<AnimalTypeEntity>> GetByIdAsync(int id);
        Task<ServiceResult<AnimalTypeEntity>> AddAsync(AnimalTypeEntity entity);
        Task<ServiceResult<AnimalTypeEntity>> UpdateAsync(int id, AnimalTypeEntity entity);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
