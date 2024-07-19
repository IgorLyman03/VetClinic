namespace Appointment.DTOs
{
    public class VetAidDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? ServiceType { get; set; } = null;
        public TimeSpan? Duration { get; set; }
        public decimal? Price { get; set; }
    }
}
