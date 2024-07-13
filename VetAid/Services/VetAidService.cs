
using Common.Results;

namespace VetAid.Services
{
    public class VetAidService : IVetAidService
    {
        private readonly IVetAidRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VetAidService> _logger;

        public VetAidService(IVetAidRepository repository, IMapper mapper, ILogger<VetAidService> logger) =>
            (_repository, _mapper, _logger) = (repository, mapper, logger);

        public async Task<ServiceResult<VetAidDto>> AddAsync(VetAidDto dto)
        {
            try
            {
                var entity = _mapper.Map<VetAidEntity>(dto);
                var result = await _repository.AddAsync(entity);
                return result.Map(addedEntity => _mapper.Map<VetAidDto>(addedEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding vet aid");
                return ServiceResult<VetAidDto>.Failure(new ServiceError("Error adding vet aid", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting vet aid with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error deleting vet aid", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<VetAidDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return result.Map(entities => _mapper.Map<IEnumerable<VetAidDto>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all vet aids");
                return ServiceResult<IEnumerable<VetAidDto>>.Failure(new ServiceError("Error getting all vet aids", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<VetAidDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                return result.Map(entity => _mapper.Map<VetAidDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vet aid with id {Id}", id);
                return ServiceResult<VetAidDto>.Failure(new ServiceError("Error getting vet aid", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<VetAidDto>> UpdateAsync(VetAidDto dto)
        {
            try
            {
                var entity = _mapper.Map<VetAidEntity>(dto);
                var result = await _repository.UpdateAsync(entity);
                return result.Map(updatedEntity => _mapper.Map<VetAidDto>(updatedEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating vet aid with id {Id}", dto.Id);
                return ServiceResult<VetAidDto>.Failure(new ServiceError("Error updating vet aid", ServiceErrorType.InternalError));
            }
        }
    }
}
