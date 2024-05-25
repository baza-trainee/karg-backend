using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.Advices;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/advice")]
    public class AdviceController : Controller
    {
        private IAdviceService _adviceService;
        private ICultureService _cultureService;

        public AdviceController(IAdviceService adviceService, ICultureService cultureService)
        {
            _adviceService = adviceService;
            _cultureService = cultureService;
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdvices([FromQuery] AdvicesFilterDTO filter, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var paginatedAdvices = await _adviceService.GetAdvices(filter, cultureCode);

                if (paginatedAdvices.Advices.Count == 0)
                {
                    return NotFound("Advices not found.");
                }

                return Ok(paginatedAdvices);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new advice.
        /// </summary>
        /// <param name="adviceDto">The data for the new advice.</param>
        /// <returns>The newly created advice.</returns>
        /// <response code="201">Returns the newly created advice.</response>
        /// <response code="500">If an error occurs while trying to create the advice.</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(CreateAndUpdateAdviceDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAndUpdateAdviceDTO adviceDto)
        {
            try
            {
                await _adviceService.CreateAdvice(adviceDto);

                return Created("CreateAdvice", adviceDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
