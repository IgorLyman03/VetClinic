namespace Appointment.Model
{
    public class BookingFilter
    {
        public string? ClientEmail { get; set; }
        public string? DoctorId { get; set; }
        public int? VetAidId { get; set; }
        public string? OwnerId { get; set; }
        public int? PetId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
