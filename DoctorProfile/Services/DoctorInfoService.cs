using AutoMapper;
using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data.Entities;
using DoctorProfile.DTOs;
using DoctorProfile.Repositories.Interfaces;
using DoctorProfile.Services.Interfaces;

namespace DoctorProfile.Services
{
    public class DoctorInfoService : IDoctorInfoService
    {
        private readonly IDoctorInfoRepository _doctorInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorInfoService> _logger;

        public DoctorInfoService(IDoctorInfoRepository doctorInfoRepository, IMapper mapper, ILogger<DoctorInfoService> logger)
        {
            _doctorInfoRepository = doctorInfoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<DoctorInfoDto>>> GetAllAsync()
        {
            try
            {
                var result = await _doctorInfoRepository.GetAllAsync();
                return result.Map(doctorInfos => _mapper.Map<IEnumerable<DoctorInfoDto>>(doctorInfos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all doctor infos");
                return ServiceResult<IEnumerable<DoctorInfoDto>>.Failure(new ServiceError("Error getting all doctor infos", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorInfoDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _doctorInfoRepository.GetByIdAsync(id);
                return result.Map(doctorInfo => _mapper.Map<DoctorInfoDto>(doctorInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting doctor info with id {Id}", id);
                return ServiceResult<DoctorInfoDto>.Failure(new ServiceError("Error getting doctor info", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorInfoDto>> GetByUserIdAsync(string userId)
        {
            try
            {
                var result = await _doctorInfoRepository.GetByUserIdAsync(userId);
                return result.Map(doctorInfo => _mapper.Map<DoctorInfoDto>(doctorInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting doctor info for user {UserId}", userId);
                return ServiceResult<DoctorInfoDto>.Failure(new ServiceError("Error getting doctor info", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorInfoDto>> AddAsync(DoctorInfoDto profile)
        {
            try
            {
                var doctorInfo = _mapper.Map<DoctorInfo>(profile);
                var result = await _doctorInfoRepository.AddAsync(doctorInfo);
                return result.Map(savedDoctorInfo => _mapper.Map<DoctorInfoDto>(savedDoctorInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding doctor info");
                return ServiceResult<DoctorInfoDto>.Failure(new ServiceError("Error adding doctor info", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorInfoDto>> UpdateAsync(int id, DoctorInfoDto profile)
        {
            try
            {
                var doctorInfo = _mapper.Map<DoctorInfo>(profile);
                var result = await _doctorInfoRepository.UpdateAsync(id, doctorInfo);
                return result.Map(updatedDoctorInfo => _mapper.Map<DoctorInfoDto>(updatedDoctorInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating doctor info with id {Id}", profile.Id);
                return ServiceResult<DoctorInfoDto>.Failure(new ServiceError("Error updating doctor info", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _doctorInfoRepository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting doctor info with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error deleting doctor info", ServiceErrorType.InternalError));
            }
        }
    }
}
