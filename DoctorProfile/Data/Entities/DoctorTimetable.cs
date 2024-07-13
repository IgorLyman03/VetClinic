namespace DoctorProfile.Data.Entities
{
    public class DoctorTimetable
    {
        private DateTimeOffset? _startTime;
        private DateTimeOffset? _endTime;

        public int Id { get; set; }

        public DateTimeOffset? StartTime
        {
            get => _startTime;
            set => _startTime = value?.ToUniversalTime();
        }

        public DateTimeOffset? EndTime
        {
            get => _endTime;
            set => _endTime = value?.ToUniversalTime();
        }

        public string? UserId { get; set; }
        public virtual DoctorInfo DoctorInfo { get; set; }
    }
}
