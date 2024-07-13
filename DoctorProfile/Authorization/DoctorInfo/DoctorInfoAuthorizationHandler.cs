using DoctorProfile.DTOs;
using DoctorProfile.Services;
using DoctorProfile.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DoctorProfile.Authorization.DoctorInfo
{
    public class DoctorInfoAuthorizationHandler
        : BaseEntityAuthorizationHandler<DoctorInfoOperationRequirement, DoctorInfoOperationResource, DoctorInfoDto, IDoctorInfoService>
    {
        public DoctorInfoAuthorizationHandler(IDoctorInfoService doctorInfoService)
            : base(doctorInfoService)
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

        protected override async Task<DoctorInfoDto> GetEntityByIdAsync(int entityId)
        {
            var result = await _service.GetByIdAsync(entityId);
            return result.IsSuccess ? result.Value : null;
        }

        protected override async Task<bool> ValidateEntityOwnership(DoctorInfoDto entity, string userId)
        {
            return entity.UserId == userId;
        }
    }
}
