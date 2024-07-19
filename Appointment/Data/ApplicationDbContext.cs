using Appointment.Data.Entities;
using Appointment.Data.EntityConfiguretions;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BookingConfiguration());
        }
    }
}
