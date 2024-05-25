using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.YearsResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/yearresult")]
    public class YearResultController : Controller
    {
        private IYearResultService _yearResultService;
        private ICultureService _cultureService;

        public YearResultController(IYearResultService yearResultService, ICultureService cultureService)
        {
            _yearResultService = yearResultService;
            _cultureService = cultureService;
        }

        /// <summary>
        /// Gets a list of years results filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of years results.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific years results. Default is "ua".</param>
        /// <response code="200">Successful request. Returns a list of years results with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No years results found based on the specified filters.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of years results.</response>
        /// <returns>List of years results with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllYearsResults([FromQuery] YearsResultsFilterDTO filter, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var paginatedYearsResults = await _yearResultService.GetYearsResults(filter, cultureCode);

                if (paginatedYearsResults.YearsResults.Count == 0)
                {
                    return NotFound("Years results not found.");
                }

                return Ok(paginatedYearsResults);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
