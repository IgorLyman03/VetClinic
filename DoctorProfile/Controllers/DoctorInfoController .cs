using Common.Results;
using CSharpFunctionalExtensions;
using DoctorProfile.Authorization.DoctorInfo;
using DoctorProfile.DTOs;
using DoctorProfile.Services;
using DoctorProfile.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DoctorProfile.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorInfoController : ControllerBase
    {
        public DoctorInfoController(IDoctorInfoService doctorInfoService, IAuthorizationService authorizationService)
        {
            _doctorInfoService = doctorInfoService;
            _authorizationService = authorizationService;
        }

        private readonly IDoctorInfoService _doctorInfoService;
        private readonly IAuthorizationService _authorizationService;

        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<DoctorInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorInfoDto>>> GetAll()
        {
            var result = await _doctorInfoService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorInfoDto>> GetById(int id)
        {
            var result = await _doctorInfoService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorInfoDto>> GetByUserId(string userId)
        {
            var result = await _doctorInfoService.GetByUserIdAsync(userId);
            return result.ToActionResult();
        }

        [HttpPost]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorInfoDto>> Add(DoctorInfoDto profileDto)
        {
            var resource = new DoctorInfoOperationResource(0, Authorization.OperationType.Create, dtoUserId: profileDto.UserId);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanCreateDoctorInfo");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorInfoService.AddAsync(profileDto);
            return result.ToActionResult();
        }

        [HttpPatch]
        [ProducesResponseType(typeof(DoctorInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorInfoDto>> Update(int id, DoctorInfoDto profileDto)
        {
            var resource = new DoctorInfoOperationResource(id, Authorization.OperationType.Update, dtoUserId: profileDto.UserId);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanUpdateDoctorInfo");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorInfoService.UpdateAsync(id, profileDto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resource = new DoctorInfoOperationResource(id, Authorization.OperationType.Delete);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, resource, "CanDeleteDoctorInfo");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _doctorInfoService.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}
