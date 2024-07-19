using Appointment.Clients;
using Appointment.Clients.Interfaces;
using Appointment.Data.Entities;
using Appointment.DTOs;
using Appointment.Model;
using Appointment.Repositories.Interfaces;
using Appointment.Services.Interfaces;
using AutoMapper;
using Common.Results;
using System.Linq;

namespace Appointment.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IDoctorClient _doctorClient;
        private readonly IVetAidClient _vetAidClient;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper, IDoctorClient doctorClient, IVetAidClient vetAidClient, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _doctorClient = doctorClient;
            _vetAidClient = vetAidClient;
            _logger = logger;
        }

        public async Task<ServiceResult<BookingDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _bookingRepository.GetByIdAsync(id);
                return result.Map(booking => _mapper.Map<BookingDto>(booking));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking by id {Id}", id);
                return ServiceResult<BookingDto>.Failure(new ServiceError("Error getting booking", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<BookingDto>>> GetAllAsync()
        {
            try
            {
                var result = await _bookingRepository.GetAllAsync();
                return result.Map(bookings => _mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all bookings");
                return ServiceResult<IEnumerable<BookingDto>>.Failure(new ServiceError("Error getting all bookings", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<BookingDto>> AddAsync(BookingDto bookingDto)
        {
            try
            {
                var timetableSegment = await _doctorClient.GetTimetableByDatesAsync(bookingDto.DoctorId, bookingDto.StartDate.Value, bookingDto.EndDate.Value);

                if(timetableSegment.IsFailure || timetableSegment.Value.Any( ts => ts.StartTime > bookingDto.StartDate.Value || ts.EndTime < bookingDto.EndDate.Value))
                {
                    return ServiceResult<BookingDto>.Failure(new ServiceError("This doctor is not working at this time", ServiceErrorType.InternalError));
                }

                var isFreeToBooking = await _bookingRepository.IsFreeToBooking(bookingDto.DoctorId, bookingDto.StartDate.Value, bookingDto.EndDate.Value);
                if (isFreeToBooking.IsFailure || isFreeToBooking.Value == false)
                {
                    return ServiceResult<BookingDto>.Failure(new ServiceError("The time for booking is busy or an error occurred", ServiceErrorType.InternalError));
                }

                var vetAid = await _vetAidClient.GetVetAidAsync(bookingDto.VetAidId.Value);
                if (vetAid.IsFailure || (bookingDto.StartDate.Value + vetAid.Value.Duration - bookingDto.EndDate.Value).Value.Duration().TotalSeconds > 1 )
                {
                    return ServiceResult<BookingDto>.Failure(new ServiceError("The vet aid does not exist or its duration does not match", ServiceErrorType.InternalError));
                }

                var booking = _mapper.Map<Booking>(bookingDto);
                var result = await _bookingRepository.AddAsync(booking);
                return result.Map(addedBooking => _mapper.Map<BookingDto>(addedBooking));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding booking");
                return ServiceResult<BookingDto>.Failure(new ServiceError("Error adding booking", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<BookingDto>> UpdateAsync(int id, BookingDto bookingDto)
        {
            try
            {
                var booking = _mapper.Map<Booking>(bookingDto);
                var result = await _bookingRepository.UpdateAsync(id, booking);
                return result.Map(updatedBooking => _mapper.Map<BookingDto>(updatedBooking));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking with id {Id}", id);
                return ServiceResult<BookingDto>.Failure(new ServiceError("Error updating booking", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                return await _bookingRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error deleting booking", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<BookingDto>>> GetFilteredBookingsAsync(BookingFilter filter)
        {
            try
            {
                var result = await _bookingRepository.GetFilteredBookingsAsync(
                    clientEmail: filter.ClientEmail,
                    doctorId: filter.DoctorId,
                    vetAidId: filter.VetAidId,
                    startDate: filter.StartDate,
                    endDate: filter.EndDate
                );
                return result.Map(bookings => _mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting filtered bookings");
                return ServiceResult<IEnumerable<BookingDto>>.Failure(new ServiceError("Error getting filtered bookings", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<bool>> SetStatusAsync(int id, AppointmentStatus status)
        {
            try
            {
                var booking = new Booking { Id = id, Status = status };
                var result = await _bookingRepository.UpdateAsync(id, booking);
                return result.Map(_ => true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting status for booking with id {Id}", id);
                return ServiceResult<bool>.Failure(new ServiceError("Error setting booking status", ServiceErrorType.InternalError));
            }
        }

        public async Task<ServiceResult<IEnumerable<DateTimeOffset>>> GetFreeTimeAsync(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate, int vetAidId)
        {
            try
            {
                var vetAidResult = await _vetAidClient.GetVetAidAsync(vetAidId);
                if (vetAidResult.IsFailure) 
                {
                    return ServiceResult<IEnumerable<DateTimeOffset>>.Failure(vetAidResult.Error);
                }
                var vetAidDuration = vetAidResult.Value.Duration;

                var doctorTimetablesResult = await _doctorClient.GetTimetableByDatesAsync(doctorId, startDate, endDate);
                if (doctorTimetablesResult.IsFailure)
                {
                    return ServiceResult<IEnumerable<DateTimeOffset>>.Failure(doctorTimetablesResult.Error);
                }
                var doctorTimetable = doctorTimetablesResult.Value;

                var bookingsResult = (await _bookingRepository.GetFilteredBookingsAsync(
                    clientEmail: null,
                    doctorId: doctorId,
                    vetAidId: null,
                    startDate: startDate,
                    endDate: endDate)).Map(bookings => _mapper.Map<IEnumerable<BookingDto>>(bookings));

                if (bookingsResult.IsFailure)
                {
                    return ServiceResult<IEnumerable<DateTimeOffset>>.Failure(bookingsResult.Error);
                }
                var bookings = bookingsResult.Value;

                // Get the doctor’s schedule, subtract from it the time for which users have already signed up and divide it into slots equal to vetAidDuration

                List<DateTimeOffset> availableSlots = new();

                foreach (var timetable in doctorTimetable)
                {
                    var currentTime = timetable.StartTime;
                    while (currentTime + vetAidDuration <= timetable.EndTime)
                    {
                        var notAvailableSlot = bookings.FirstOrDefault(b =>
                            currentTime < b.EndDate && currentTime + vetAidDuration > b.StartDate);

                        if (notAvailableSlot == null)
                        {
                            availableSlots.Add(currentTime);
                            currentTime = currentTime + vetAidDuration!.Value;
                        }
                        else
                        {
                            currentTime = notAvailableSlot.EndDate.Value;
                        }
                    }
                }

                return ServiceResult<IEnumerable<DateTimeOffset>>.Success(availableSlots);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting free time", doctorId);
                return ServiceResult<IEnumerable<DateTimeOffset>>.Failure(new ServiceError("Error getting free time", ServiceErrorType.InternalError));
            }
        }
    }
}
