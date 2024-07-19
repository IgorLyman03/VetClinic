using Microsoft.AspNetCore.Authorization;

namespace VetAid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VetAidController : ControllerBase
    {

        public VetAidController(IVetAidService vetAidService) =>
            (_vetAidService) = (vetAidService);
        

        private readonly IVetAidService _vetAidService;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VetAidDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vetAidService.GetAllAsync();
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VetAidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _vetAidService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(VetAidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(VetAidDto dto)
        {
            var result = await _vetAidService.AddAsync(dto);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(VetAidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,VetAidDto dto)
        {
            var result = await _vetAidService.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(VetAidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _vetAidService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.ErrorCode, result.Error);
        }
    }
}
