using AutoMapper;
using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Data;
using DoctorProfile.Data.Entities;
using DoctorProfile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static Common.Helpers.RepositoryHelper;

namespace DoctorProfile.Repositories
{
    public class DoctorInfoRepository: IDoctorInfoRepository
    {

        public DoctorInfoRepository(ApplicationDbContext context) => 
            (_context) = (context);
        

        private readonly ApplicationDbContext _context;

        public async Task<ServiceResult<IEnumerable<DoctorInfo>>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var doctors = await _context.DoctorInfos.ToListAsync();
                return ServiceResult<IEnumerable<DoctorInfo>>.Success(doctors);
            });
        }

        public async Task<ServiceResult<DoctorInfo>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var doctor = await _context.DoctorInfos.FirstOrDefaultAsync(di => di.Id == id);
                if (doctor == null)
                {
                    return ServiceResult<DoctorInfo>.Failure("Doctor info not found", ServiceErrorType.NotFound);
                }
                return ServiceResult<DoctorInfo>.Success(doctor);
            });
        }

        public async Task<ServiceResult<DoctorInfo>> GetByUserIdAsync(string userId)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var doctor = await _context.DoctorInfos.FirstOrDefaultAsync(di => di.UserId == userId);
                if (doctor == null)
                {
                    return ServiceResult<DoctorInfo>.Failure("Doctor info not found", ServiceErrorType.NotFound);
                }
                return ServiceResult<DoctorInfo>.Success(doctor);
            });
        }

        public async Task<ServiceResult<DoctorInfo>> AddAsync(DoctorInfo profile)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var ServiceResult = await _context.DoctorInfos.AddAsync(profile);
                await _context.SaveChangesAsync();
                return ServiceResult<DoctorInfo>.Success(ServiceResult.Entity);
            });
        }

        public async Task<ServiceResult<DoctorInfo>> UpdateAsync(int id, DoctorInfo profile)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var existingProfile = await _context.DoctorInfos.FindAsync(id);
                if (existingProfile == null)
                {
                    return ServiceResult<DoctorInfo>.Failure("Doctor info not found", ServiceErrorType.NotFound);
                }

                if (profile.FullName != null)
                {
                    existingProfile.FullName = profile.FullName;
                }
                if (profile.Specialization != null)
                {
                    existingProfile.Specialization = profile.Specialization;
                }

                await _context.SaveChangesAsync();
                return ServiceResult<DoctorInfo>.Success(existingProfile);
            });
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var existingProfile = await _context.DoctorInfos.FindAsync(id);
                if (existingProfile == null)
                {
                    return ServiceResult <bool>.Failure("Doctor info not found", ServiceErrorType.NotFound);
                }

                _context.DoctorInfos.Remove(existingProfile);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true);
            });
        }

    }
}
