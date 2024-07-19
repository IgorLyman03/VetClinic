using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data;
using DoctorProfile.Data.Entities;
using DoctorProfile.Model;
using DoctorProfile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Common.Helpers.RepositoryHelper;

namespace DoctorProfile.Repositories
{
    public class DoctorTimetableRepository : IDoctorTimetableRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorTimetableRepository(ApplicationDbContext context) =>
            (_context) = (context);

        public async Task<ServiceResult<IEnumerable<DoctorTimetable>>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var timetables = await _context.DoctorTimetables.ToListAsync();
                return ServiceResult<IEnumerable<DoctorTimetable>>.Success(timetables);
            });
        }

        public async Task<ServiceResult<DoctorTimetable>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var timetable = await _context.DoctorTimetables.FirstOrDefaultAsync(dt => dt.Id == id);
                if (timetable == null)
                {
                    return ServiceResult<DoctorTimetable>.Failure("Doctor timetable not found", ServiceErrorType.NotFound);
                }
                return ServiceResult<DoctorTimetable>.Success(timetable);
            });
        }

        public async Task<ServiceResult<IEnumerable<DoctorTimetable>>> GetByUserIdAsync(string userId)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var timetables = await _context.DoctorTimetables.Where(dt => dt.UserId == userId).ToListAsync();
                return ServiceResult<IEnumerable<DoctorTimetable>>.Success(timetables.AsEnumerable());
            });
        }

        public async Task<ServiceResult<IEnumerable<DoctorTimetableSegment>>> GetSegmentsByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var timetables = await _context.DoctorTimetables
                    .Where(dt => dt.UserId == userId && dt.StartTime <= endDate && dt.EndTime >= startDate)
                    .ToListAsync();

                var segments = timetables.Select(dt => new DoctorTimetableSegment(
                    dt.UserId,
                    dt.StartTime.Value > startDate ? dt.StartTime.Value : startDate,
                    dt.EndTime.Value < endDate ? dt.EndTime.Value : endDate
                ));

                return ServiceResult<IEnumerable<DoctorTimetableSegment>>.Success(segments);
            });
        }

        public async Task<ServiceResult<DoctorTimetable>> AddAsync(DoctorTimetable timetable)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var ServiceResult = await _context.DoctorTimetables.AddAsync(timetable);
                await _context.SaveChangesAsync();
                return ServiceResult<DoctorTimetable>.Success(ServiceResult.Entity);
            });
        }

        public async Task<ServiceResult<DoctorTimetable>> UpdateAsync(int id, DoctorTimetable timetable)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var existingTimetable = await _context.DoctorTimetables.FindAsync(id);

                if (existingTimetable == null)
                {
                    return ServiceResult<DoctorTimetable>.Failure("Doctor timetable not found", ServiceErrorType.NotFound);
                }

                if (timetable.StartTime != default)
                {
                    existingTimetable.StartTime = timetable.StartTime;
                }
                if (timetable.EndTime != default)
                {
                    existingTimetable.EndTime = timetable.EndTime;
                }

                return ServiceResult<DoctorTimetable>.Success(existingTimetable);
            });
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var existingTimetable = await _context.DoctorTimetables.FindAsync(id);
                if (existingTimetable == null)
                {
                    return ServiceResult<bool>.Failure("Doctor timetable not found", ServiceErrorType.NotFound);
                }

                _context.DoctorTimetables.Remove(existingTimetable);
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.Success(true);
            });
        }
    }
}
