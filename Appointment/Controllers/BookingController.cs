using Appointment.Clients;
using Appointment.Data.Entities;
using Appointment.DTOs;
using Appointment.Model;
using Appointment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Results;

namespace Appointment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAuthorizationService _authorizationService;

        public BookingController(IBookingService bookingService, IAuthorizationService authorizationService)
        {
            _bookingService = bookingService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var result = await _bookingService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> Add(BookingDto booking)
        {
            /*
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, booking, "CanCreateBooking");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }
            */
            var result = await _bookingService.AddAsync(booking);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> Update(int id, BookingDto booking)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, booking, "CanUpdateBooking");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _bookingService.UpdateAsync(id, booking);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, id, "CanDeleteBooking");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _bookingService.DeleteAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("filtered")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetFilteredBookings([FromQuery] BookingFilter filter)
        {
            var result = await _bookingService.GetFilteredBookingsAsync(filter);
            return result.ToActionResult();
        }

        [HttpGet("free-time")]
        [ProducesResponseType(typeof(IEnumerable<DateTimeOffset>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DateTimeOffset>>> GetFreeTime(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate, int vetAidId)
        {
            var result = await _bookingService.GetFreeTimeAsync(doctorId, startDate, endDate, vetAidId);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> SetStatus(int id, AppointmentStatus status)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, id, "CanUpdateBookingStatus");
            if (!isAuthorized.Succeeded)
            {
                return Unauthorized("User is not authorized to perform this action.");
            }

            var result = await _bookingService.SetStatusAsync(id, status);
            return result.ToActionResult();
        }
    }
}
