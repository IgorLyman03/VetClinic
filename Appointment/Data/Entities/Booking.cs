namespace Appointment.Data.Entities
{
    public class Booking
    {
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;
        public int? Id { get; set; }
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => _startDate = value?.ToUniversalTime();
        }
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => _endDate = value?.ToUniversalTime();
        }
        public string? ClientEmail { get; set; }
        public string? DoctorId { get; set; }
        public int? VetAidId { get; set; }
        public int? PetType { get; set; }
        public string? PetBreed { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? BookingNode { get; set; }
    }
}
