namespace Appointment.Authorization.Booking
{
    public class BookingOperationRequirement : BaseOperationRequirement
    {
        public BookingOperationRequirement(OperationType operationType) : base(operationType)
        {
        }
    }
}
