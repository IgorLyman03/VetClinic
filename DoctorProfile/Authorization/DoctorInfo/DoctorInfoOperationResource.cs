namespace DoctorProfile.Authorization.DoctorInfo
{
    public class DoctorInfoOperationResource
    {
        public int DoctorInfoId { get; set; }
        public OperationType OperationType { get; set; }
        public string? DtoUserId { get; set; }

        public DoctorInfoOperationResource(int doctorInfoId, OperationType operationType, string? dtoUserId = null)
        {
            DoctorInfoId = doctorInfoId;
            OperationType = operationType;
            DtoUserId = dtoUserId;
        }
    }
}
