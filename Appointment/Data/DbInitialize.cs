using Appointment.Data;
using Appointment.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}
