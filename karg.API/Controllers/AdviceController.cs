using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.Advices;
using karg.BLL.DTO.Advices;

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
    }
}
