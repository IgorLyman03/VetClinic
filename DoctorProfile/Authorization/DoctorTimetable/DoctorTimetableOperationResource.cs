namespace DoctorProfile.Authorization.DoctorTimetable
{
    public class DoctorTimetableOperationResource
    {
        public int DoctorTimetableId { get; set; }
        public OperationType OperationType { get; set; }
        public string? DtoUserId { get; set; }

        public DoctorTimetableOperationResource(int doctorTimetableId, OperationType operationType, string? dtoUserId = null)
        {
            DoctorTimetableId = doctorTimetableId;
            OperationType = operationType;
            DtoUserId = dtoUserId;
        }
    }
}
