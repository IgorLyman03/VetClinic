using Appointment.DTOs;
using Appointment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Appointment.Authorization.Booking
{
    public class DoctorInfoAuthorizationHandler
        : BaseEntityAuthorizationHandler<BookingOperationRequirement, BookingOperationResource, BookingDto, IBookingService>
    {
        public DoctorInfoAuthorizationHandler(IBookingService bookingService)
            : base(bookingService)
        {
        }

        protected override async Task HandleDoctorRoleAsync(
            AuthorizationHandlerContext context,
            BookingOperationRequirement requirement,
            BookingOperationResource resource,
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
                    var contextUserForDelete = await GetEntityByIdAsync(resource.BookingId);
                    if (contextUserForDelete != null && await ValidateEntityOwnership(contextUserForDelete, userId))
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case OperationType.Update:
                    if (resource.DtoUserId != null)
                    {
                        var contextUserForUpdate = await GetEntityByIdAsync(resource.BookingId);
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
                case OperationType.PartialUpdate:
                    var contextUserForPartialUpdate = await GetEntityByIdAsync(resource.BookingId);
                    if (contextUserForPartialUpdate != null && await ValidateEntityOwnership(contextUserForPartialUpdate, userId))
                    {
                        context.Succeed(requirement);
                    }
                    break;
            }
        }

        protected override async Task<BookingDto> GetEntityByIdAsync(int entityId)
        {
            var result = await _service.GetByIdAsync(entityId);
            return result.IsSuccess ? result.Value : null;
        }

        protected override async Task<bool> ValidateEntityOwnership(BookingDto entity, string userId)
        {
            return entity.DoctorId == userId;
        }
    }
}
