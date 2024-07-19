using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Appointment.Authorization
{
    public enum OperationType
    {
        Create,
        Read,
        Update,
        PartialUpdate,
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
