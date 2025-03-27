using karg.BLL.DTO.Utilities;
using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/yearresult")]
    public class YearResultController : Controller
    {
        private IYearResultService _yearResultService;
        private ICultureService _cultureService;
        private readonly ILogger<YearResultController> _logger;

        public YearResultController(IYearResultService yearResultService, ICultureService cultureService, ILogger<YearResultController> logger)
        {
            _yearResultService = yearResultService;
            _cultureService = cultureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of years results filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of years results.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific years results.</param>
        /// <response code="200">Successful request. Returns a list of years results with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of years results.</response>
        /// <returns>List of years results with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllYearsResults([FromQuery] YearsResultsFilterDTO filter, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching all year results with filter: {@Filter} and culture: {CultureCode}", JsonSerializer.Serialize(filter), cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching all year results.");
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var paginatedYearResults = await _yearResultService.GetYearsResults(filter, cultureCode);

            _logger.LogInformation("Successfully retrieved {Count} year results", paginatedYearResults.Items.Count);
            return StatusCode(StatusCodes.Status200OK, paginatedYearResults);
        }

        /// <summary>
        /// Gets the year result details by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the year result.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details. Default is "ua".</param>
        /// <response code="200">Successful request. Returns the details of the specified year result.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No year result found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the year result details.</response>
        /// <returns>The details of the specified year result.</returns>
        [HttpGet("getbyid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYearResultById(int id, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching year result with ID: {Id} and culture: {CultureCode}", id, cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching year result ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var yearResult = await _yearResultService.GetYearResultById(id, cultureCode);

            if (yearResult == null)
            {
                _logger.LogWarning("Year result with ID: {Id} not found", id);
                return StatusCode(StatusCodes.Status404NotFound, "Підсумок не знайдено.");
            }

            _logger.LogInformation("Successfully retrieved year result with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, yearResult);
        }

        /// <summary>
        /// Creates a new year result.
        /// </summary>
        /// <param name="yearResultDto">The data for the new year result.</param>
        /// <returns>The newly created year result.</returns>
        /// <response code="201">Returns the newly created year result.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">If an error occurs while trying to create the year result.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdateYearResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateYearResult([FromBody] CreateAndUpdateYearResultDTO yearResultDto)
        {
            _logger.LogInformation("Creating a new year result: {@YearResult}", JsonSerializer.Serialize(yearResultDto));

            var newYearResult = await _yearResultService.CreateYearResult(yearResultDto);

            _logger.LogInformation("Successfully created new year result.");
            return StatusCode(StatusCodes.Status201Created, newYearResult);
        }

        /// <summary>
        /// Updates the details of a specific year result.
        /// </summary>
        /// <param name="id">The unique identifier of the year result to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the year result.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the year result details.</response>
        /// <returns>The updated details of the year result.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateYearResult(int id, [FromBody] JsonPatchDocument<CreateAndUpdateYearResultDTO> patchDoc)
        {
            _logger.LogInformation("Updating year result with ID: {Id}", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Invalid request for updating year result with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Недійсний запит.");
            }

            var updatedYearResult = await _yearResultService.UpdateYearResult(id, patchDoc);

            _logger.LogInformation("Successfully updated year result with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, updatedYearResult);
        }

        /// <summary>
        /// Deletes a specific year result.
        /// </summary>
        /// <param name="id">The unique identifier of the year result to be deleted.</param>
        /// <response code="204">Successful request. The year result has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">An internal server error occurred while trying to delete the year result.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteYearResult(int id)
        {
            _logger.LogInformation("Deleting year result with ID: {Id}", id);

            await _yearResultService.DeleteYearResult(id);

            _logger.LogInformation("Successfully deleted year result with ID: {Id}", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
