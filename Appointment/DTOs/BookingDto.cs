namespace Appointment.DTOs
{
    public class BookingDto
    {
        public int? Id { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? ClientEmail { get; set; }
        public string? DoctorId { get; set; }
        public int? VetAidId { get; set; }
        public int? PetType { get; set; }
        public string? PetBreed { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? BookingNode { get; set; }
    }
}
