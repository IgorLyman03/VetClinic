namespace DoctorProfile.DTOs
{
    public class DoctorTimetableSegmentDto
    {
        public string UserId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
