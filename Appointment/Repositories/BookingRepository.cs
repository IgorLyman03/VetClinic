using Appointment.Data;
using Appointment.Data.Entities;
using Appointment.Repositories.Interfaces;
using Common.Results;
using static Common.Helpers.RepositoryHelper;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<IEnumerable<Booking>>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _context.Bookings.ToListAsync();
                return ServiceResult<IEnumerable<Booking>>.Success(result);
            });
        }

        public async Task<ServiceResult<Booking>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
                if (result == null)
                {
                    return ServiceResult<Booking>.Failure("Booking not found", ServiceErrorType.NotFound);
                }
                return ServiceResult<Booking>.Success(result);
            });
        }

        public async Task<ServiceResult<Booking>> AddAsync(Booking booking)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = _context.Bookings.Add(booking).Entity;
                await _context.SaveChangesAsync();
                return ServiceResult<Booking>.Success(result);
            });

        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return ServiceResult<bool>.Failure(new ServiceError("Booking not found", ServiceErrorType.NotFound));
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.Success(true);
            });
        }
        public async Task<ServiceResult<Booking>> UpdateAsync(int id, Booking booking)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var existingBooking = await _context.Bookings.FindAsync(id);

                if (existingBooking == null)
                {
                    return ServiceResult<Booking>.Failure(new ServiceError($"Booking with id {id} not found", ServiceErrorType.NotFound));
                }

                if (booking.StartDate != null)
                {
                    existingBooking.StartDate = booking.StartDate;
                }
                if (booking.EndDate != null)
                {
                    existingBooking.EndDate = booking.EndDate;
                }
                if (booking.ClientEmail != null)
                {
                    existingBooking.ClientEmail = booking.ClientEmail;
                }
                if (booking.DoctorId != null)
                {
                    existingBooking.DoctorId = booking.DoctorId;
                }
                if (booking.VetAidId != null)
                {
                    existingBooking.VetAidId = booking.VetAidId;
                }
                if (booking.Status != null)
                {
                    existingBooking.Status = booking.Status;
                }
                if (booking.BookingNode != null)
                {
                    existingBooking.BookingNode = booking.BookingNode;
                }

                await _context.SaveChangesAsync();

                return ServiceResult<Booking>.Success(existingBooking);
            });
        }
        public async Task<ServiceResult<IEnumerable<Booking>>> GetFilteredBookingsAsync(string? clientEmail, string? doctorId, int? vetAidId, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var query = _context.Bookings.AsQueryable();

                if (!string.IsNullOrEmpty(clientEmail))
                {
                    query = query.Where(b => b.ClientEmail == clientEmail);
                }
                if (doctorId != null)
                {
                    query = query.Where(b => b.DoctorId == doctorId);
                }
                if (vetAidId.HasValue)
                {
                    query = query.Where(b => b.VetAidId == vetAidId);
                }
                if (startDate.HasValue)
                {
                    query = query.Where(b => b.StartDate >= startDate);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(b => b.EndDate <= endDate);
                }

                var result = await query.ToListAsync();
                return ServiceResult<IEnumerable<Booking>>.Success(result);
            });
        }
        public async Task<ServiceResult<bool>> IsFreeToBooking(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = _context.Bookings.Any(b => b.DoctorId == doctorId && b.StartDate <= endDate && b.EndDate >= startDate);
                return ServiceResult<bool>.Success(result);
            });
        }
    }
}
