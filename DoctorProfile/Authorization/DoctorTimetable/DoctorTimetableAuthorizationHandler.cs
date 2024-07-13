using DoctorProfile.Authorization.DoctorInfo;
using DoctorProfile.DTOs;
using DoctorProfile.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DoctorProfile.Authorization.DoctorTimetable
{
    public class DoctorTimetableAuthorizationHandler
        : BaseEntityAuthorizationHandler<DoctorInfoOperationRequirement, DoctorInfoOperationResource, DoctorTimetableDto, IDoctorTimetableService>
    {
        public DoctorTimetableAuthorizationHandler(IDoctorTimetableService doctorTimetableService)
            : base(doctorTimetableService)
        {
        }

        protected override async Task HandleDoctorRoleAsync(
            AuthorizationHandlerContext context,
            DoctorInfoOperationRequirement requirement,
            DoctorInfoOperationResource resource,
            string userId)
        {
            switch (resource.OperationType)
            {
                case OperationType.Create:
                    if (resource.DtoUserId == userId)
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case OperationType.Delete:
                    var contextUser = await GetEntityByIdAsync(resource.DoctorInfoId);
                    if (contextUser != null && await ValidateEntityOwnership(contextUser, userId))
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case OperationType.Update:
                    if (resource.DtoUserId != null)
                    {
                        var contextUserForUpdate = await GetEntityByIdAsync(resource.DoctorInfoId);
                        if (contextUserForUpdate != null &&
                            await ValidateEntityOwnership(contextUserForUpdate, userId) &&
                            resource.DtoUserId == userId)
                        {
                            context.Succeed(requirement);
                        }
                    }
                    else if (resource.DtoUserId == userId)
                    {
                        context.Succeed(requirement);
                    }
                    break;
            }
        }

        protected override async Task<DoctorTimetableDto> GetEntityByIdAsync(int entityId)
        {
            var result = await _service.GetByIdAsync(entityId);
            return result.IsSuccess ? result.Value : null;
        }

        protected override async Task<bool> ValidateEntityOwnership(DoctorTimetableDto entity, string userId)
        {
            return entity.UserId == userId;
        }
    }
}
