using AutoMapper;
using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data.Entities;
using DoctorProfile.DTOs;
using DoctorProfile.Repositories.Interfaces;
using DoctorProfile.Services.Interfaces;
using System.Numerics;

namespace DoctorProfile.Services
{
    public class DoctorTimetableService : IDoctorTimetableService
    {
        private readonly IDoctorTimetableRepository _doctorTimetableRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorTimetableService> _logger;

        public DoctorTimetableService(IDoctorTimetableRepository doctorTimetableRepository, IMapper mapper, ILogger<DoctorTimetableService> logger)
        {
            _doctorTimetableRepository = doctorTimetableRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<DoctorTimetableDto>>> GetAllAsync()
        {
            try
            {
                var result = await _doctorTimetableRepository.GetAllAsync();
                return result.Map(timetables => _mapper.Map<IEnumerable<DoctorTimetableDto>>(timetables));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all doctor timetables");
                return ServiceResult<IEnumerable<DoctorTimetableDto>>.Failure(new ServiceError("Error getting all doctor timetables", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorTimetableDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _doctorTimetableRepository.GetByIdAsync(id);
                return result.Map(timetable => _mapper.Map<DoctorTimetableDto>(timetable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting doctor timetable with id {Id}", id);
                return ServiceResult<DoctorTimetableDto>.Failure(new ServiceError("Error getting doctor timetable", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorTimetableDto>> AddAsync(DoctorTimetableDto timetableDto)
        {
            try
            {
                var timetable = _mapper.Map<DoctorTimetable>(timetableDto);
                var result = await _doctorTimetableRepository.AddAsync(timetable);
                return result.Map(addedTimetable => _mapper.Map<DoctorTimetableDto>(addedTimetable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding doctor timetable");
                return ServiceResult<DoctorTimetableDto>.Failure(new ServiceError("Error adding doctor timetable", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<DoctorTimetableDto>> UpdateAsync(int id, DoctorTimetableDto timetableDto)
        {
            try
            {
                var timetable = _mapper.Map<DoctorTimetable>(timetableDto);
                var result = await _doctorTimetableRepository.UpdateAsync(id, timetable);
                return result.Map(updatedTimetable => _mapper.Map<DoctorTimetableDto>(updatedTimetable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating doctor timetable with id {Id}", timetableDto.Id);
                return ServiceResult<DoctorTimetableDto>.Failure(new ServiceError("Error updating doctor timetable", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _doctorTimetableRepository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting doctor timetable with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error deleting doctor timetable", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<DoctorTimetableDto>>> GetByUserIdAsync(string userId)
        {
            try
            {
                var result = await _doctorTimetableRepository.GetByUserIdAsync(userId);
                return result.Map(timetables => _mapper.Map<IEnumerable<DoctorTimetableDto>>(timetables));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting doctor timetables for user {UserId}", userId);
                return ServiceResult<IEnumerable<DoctorTimetableDto>>.Failure(new ServiceError("Error getting doctor timetables", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>> GetSegmentsByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            try
            {
                var result = await _doctorTimetableRepository.GetSegmentsByDatesAsync(userId, startDate, endDate);
                return result.Map(timetables => _mapper.Map<IEnumerable<DoctorTimetableSegmentDto>>(timetables));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting doctor timetables for user {UserId} between dates {StartDate} and {EndDate}", userId, startDate, endDate);
                return ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Failure(new ServiceError("Error getting doctor timetables", ServiceErrorType.InternalError));
            }
        }
    }
}
