using DoctorProfile.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorProfile.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.DoctorInfos.Any(p => p.Email == "doctor1@example.com"))
            {
                var doctorInfo = new DoctorInfo
                {
                    UserId = "18403cdd-cb04-411e-afe5-fe8aa5ed30a3",
                    FullName = "Doctor 1",
                    Email = "doctor1@example.com",
                    Specialization = "Your Specialization"
                };
                context.DoctorInfos.Add(doctorInfo);
                await context.SaveChangesAsync();
            }

            var allEntries = context.DoctorTimetables.ToList();
            context.DoctorTimetables.RemoveRange(allEntries);
            context.SaveChanges();

            var doctor = context.DoctorInfos.FirstOrDefault(p => p.Email == "doctor1@example.com");
            if (doctor != null && !context.DoctorTimetables.Any())
            {
                for (int i = 0; i < 30; i++)
                {
                    var availability = new DoctorTimetable
                    {
                        UserId = doctor.UserId,
                        StartTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(3)).Date.AddDays(i).AddHours(9),
                        EndTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(3)).Date.AddDays(i).AddHours(17),
                    };
                    context.DoctorTimetables.Add(availability);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
