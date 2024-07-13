using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

public abstract class BaseEntityAuthorizationHandler<TRequirement, TResource, TEntity, TService>
    : AuthorizationHandler<TRequirement, TResource>
    where TRequirement : IAuthorizationRequirement
    where TResource : class
    where TEntity : class
    where TService : class
{
    protected readonly TService _service;

    protected BaseEntityAuthorizationHandler(TService service)
    {
        _service = service;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TResource resource)
    {
        var user = context.User;
        if (user.IsInRole("admin"))
        {
            context.Succeed(requirement);
            return;
        }

        var userIdClaim = user.FindFirst(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (userIdClaim == null)
        {
            return;
        }

        if (user.IsInRole("doctor"))
        {
            await HandleDoctorRoleAsync(context, requirement, resource, userIdClaim.Value);
        }

        if (user.IsInRole("user"))
        {
            await HandleUserRoleAsync(context, requirement, resource, userIdClaim.Value);
        }

    }

    protected async virtual Task HandleDoctorRoleAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TResource resource,
        string userId)
    {
        await Task.CompletedTask;
    }

    protected async virtual Task HandleUserRoleAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TResource resource,
        string userId)
    {
        await Task.CompletedTask;
    }

    protected abstract Task<TEntity> GetEntityByIdAsync(int entityId);

    protected async virtual Task<bool> ValidateEntityOwnership(TEntity entity, string userId)
    {
        return false;
    }
}