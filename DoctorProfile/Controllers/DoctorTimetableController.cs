using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Authorization.DoctorInfo;
using DoctorProfile.Authorization.DoctorTimetable;
using DoctorProfile.DTOs;
using DoctorProfile.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorTimetableController : ControllerBase
    {
        public DoctorTimetableController(IDoctorTimetableService doctorTimetableService, IAuthorizationService authorizationService)
        {
            _doctorTimetableService = doctorTimetableService;
            _authorizationService = authorizationService;
        }

        private readonly IDoctorTimetableService _doctorTimetableService;
        private readonly IAuthorizationService _authorizationService;


        [HttpGet("All")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorTimetableDto>>> GetAll()
        {
            var result = await _doctorTimetableService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorTimetableDto>> GetById(int id)
        {
            var result = await _doctorTimetableService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("ByUserId")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorTimetableDto>>> GetByUserId(string profileId)
        {
            var result = await _doctorTimetableService.GetByUserIdAsync(profileId);
            return result.ToActionResult();
        }

        [HttpGet("ByDates")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorTimetableDto>>> GetByDates(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = await _doctorTimetableService.GetByDatesAsync(userId, startDate, endDate);
            return result.ToActionResult();
        }

        [HttpPost]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorTimetableDto>> Add(DoctorTimetableDto timetableDto)
        {
            var resource = new DoctorTimetableOperationResource(0, Authorization.OperationType.Create, dtoUserId: timetableDto.UserId);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanCreateDoctorTimetable");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorTimetableService.AddAsync(timetableDto);
            return result.ToActionResult();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorTimetableDto>> Update(int id, DoctorTimetableDto timetableDto)
        {
            var resource = new DoctorTimetableOperationResource(id, Authorization.OperationType.Update, dtoUserId: timetableDto.UserId);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanUpdateDoctorTimetable");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorTimetableService.UpdateAsync(id, timetableDto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var resource = new DoctorTimetableOperationResource(id, Authorization.OperationType.Delete);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanDeleteDoctorTimetable");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorTimetableService.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}
