namespace DoctorProfile.Data.Entities
{
    public class DoctorInfo
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<DoctorTimetable> DoctorTimetables { get; set; }
    }
}
