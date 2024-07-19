using Microsoft.OpenApi.Models;

namespace Appointment.Authorization.Booking
{
    public class BookingOperationResource
    {
        public int BookingId { get; set; }
        public OperationType OperationType { get; set; }
        public string? DtoUserId { get; set; }
        public string? DtoDoctorId { get; set; }

        public BookingOperationResource(int bookingId, OperationType operationType, string? dtoUserId = null, string? dtoDoctorId = null)
        {
            BookingId = bookingId;
            OperationType = operationType;
            DtoUserId = dtoUserId;
            DtoDoctorId = dtoDoctorId;
        }
    }
}
