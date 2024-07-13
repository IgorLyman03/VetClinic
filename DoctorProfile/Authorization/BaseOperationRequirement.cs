using Microsoft.AspNetCore.Authorization;

namespace DoctorProfile.Authorization
{
    public enum OperationType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class BaseOperationRequirement : IAuthorizationRequirement
    {
        public OperationType OperationType { get; }

        public BaseOperationRequirement(OperationType operationType)
        {
            OperationType = operationType;
        }
    }
}
