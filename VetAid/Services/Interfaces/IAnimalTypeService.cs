using Common.Results;

namespace VetAid.Services.Interfaces
{
    public interface IAnimalTypeService
    {
        Task<ServiceResult<IEnumerable<AnimalTypeDto>>> GetAllAsync();
        Task<ServiceResult<AnimalTypeDto>> GetByIdAsync(int id);
        Task<ServiceResult<AnimalTypeDto>> AddAsync(AnimalTypeDto dto);
        Task<ServiceResult<AnimalTypeDto>> UpdateAsync(AnimalTypeDto dto);
        Task<ServiceResult<bool>?> DeleteAsync(int id);
    }
}
