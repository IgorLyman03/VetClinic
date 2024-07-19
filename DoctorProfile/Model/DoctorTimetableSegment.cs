namespace DoctorProfile.Model
{
    public class DoctorTimetableSegment
    {
        public DoctorTimetableSegment(string userId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            UserId = userId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public string UserId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
