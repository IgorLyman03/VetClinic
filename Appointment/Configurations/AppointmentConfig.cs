namespace Appointment.Configurations;
public class AppointmentConfig
{
    public string ConnectionString { get; set; } = null!;
    public string DoctorApiUrl { get; set; }
    public string VetAidApiUrl { get; set; }
}