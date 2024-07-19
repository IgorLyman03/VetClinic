
using Common.Results;

namespace VetAid.Services
{
    public class AnimalTypeService : IAnimalTypeService
    {
        private readonly IAnimalTypeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AnimalTypeService> _logger;

        public AnimalTypeService(IAnimalTypeRepository repository, IMapper mapper, ILogger<AnimalTypeService> logger) =>
            (_repository, _mapper, _logger) = (repository, mapper, logger);
        public async Task<ServiceResult<AnimalTypeDto>> AddAsync(AnimalTypeDto dto)
        {
            try
            {
                var entity = _mapper.Map<AnimalTypeEntity>(dto);
                var result = await _repository.AddAsync(entity);
                return result.Map(addedEntity => _mapper.Map<AnimalTypeDto>(addedEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding animal type");
                return ServiceResult<AnimalTypeDto>.Failure(new ServiceError("Error adding animal type", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting animal type with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error deleting animal type", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<AnimalTypeDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return result.Map(entities => _mapper.Map<IEnumerable<AnimalTypeDto>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all animal types");
                return ServiceResult<IEnumerable<AnimalTypeDto>>.Failure(new ServiceError("Error getting all animal types", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<AnimalTypeDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                return result.Map(entity => _mapper.Map<AnimalTypeDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting animal type with id {Id}", id);
                return ServiceResult<AnimalTypeDto>.Failure(new ServiceError("Error getting animal type", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<AnimalTypeDto>> UpdateAsync(int id, AnimalTypeDto dto)
        {
            try
            {
                var entity = _mapper.Map<AnimalTypeEntity>(dto);
                var result = await _repository.UpdateAsync(id, entity);
                return result.Map(updatedEntity => _mapper.Map<AnimalTypeDto>(updatedEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating animal type with id {Id}", dto.Id);
                return ServiceResult<AnimalTypeDto>.Failure(new ServiceError("Error updating animal type", ServiceErrorType.InternalError));
            }
        }
    }
}
