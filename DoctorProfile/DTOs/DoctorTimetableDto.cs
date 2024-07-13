using DoctorProfile.Data.Entities;

namespace DoctorProfile.DTOs
{
    public class DoctorTimetableDto
    {
        public int Id { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public string? UserId { get; set; }
    }
}
