using DoctorProfile.Data.Entities;
using DoctorProfile.Data.EntityConfiguretions;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DoctorProfile.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<DoctorTimetable> DoctorTimetables { get; set; } = null!;
        public DbSet<DoctorInfo> DoctorInfos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DoctorTimetableConfiguration());
            builder.ApplyConfiguration(new DoctorInfoConfiguration());
        }
    }
}
