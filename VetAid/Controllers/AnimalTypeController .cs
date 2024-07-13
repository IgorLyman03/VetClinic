using Common.Results;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VetAid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalTypeController : ControllerBase
    {
        private readonly IAnimalTypeService _animalTypeService;

        public AnimalTypeController(IAnimalTypeService animalTypeService) =>
            (_animalTypeService) = (animalTypeService);


        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<AnimalTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _animalTypeService.GetAllAsync();
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpGet("ById")]
        [ProducesResponseType(typeof(AnimalTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _animalTypeService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AnimalTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddAsync([FromBody] AnimalTypeDto animalTypeDto)
        {
            var result = await _animalTypeService.AddAsync(animalTypeDto);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AnimalTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(AnimalTypeDto animalTypeDto)
        {
            var result = await _animalTypeService.UpdateAsync(animalTypeDto);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(AnimalTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _animalTypeService.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : StatusCode(result.Error.ErrorCode, result.Error);
        }
    }
}
