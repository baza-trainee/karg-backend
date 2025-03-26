using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using karg.BLL.DTO.Advices;
using Microsoft.AspNetCore.JsonPatch;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using System.Text.Json;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/advice")]
    public class AdviceController : Controller
    {
        private IAdviceService _adviceService;
        private ICultureService _cultureService;
        private readonly ILogger<AdviceController> _logger;

        public AdviceController(IAdviceService adviceService, ICultureService cultureService, ILogger<AdviceController> logger)
        {
            _adviceService = adviceService;
            _cultureService = cultureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of advices filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of advices.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific advices.</param>
        /// <response code="200">Successful request. Returns a list of advices with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of advices.</response>
        /// <returns>List of advices with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdvices([FromQuery] AdvicesFilterDTO filter, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching all advices with filter: {FilterJson} and culture: {CultureCode}", JsonSerializer.Serialize(filter), cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching all advices.");
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var paginatedAdvices = await _adviceService.GetAdvices(filter, cultureCode);

            _logger.LogInformation("Successfully retrieved {Count} advices", paginatedAdvices.Items.Count);
            return StatusCode(StatusCodes.Status200OK, paginatedAdvices);
        }

        /// <summary>
        /// Gets the details of a specific advice by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the advice.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details.</param>
        /// <response code="200">Successful request. Returns the details of the specified advice.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No advice found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the advice details.</response>
        /// <returns>The details of the specified advice.</returns>
        [HttpGet("getbyid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdviceById(int id, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching advice with ID: {Id} and culture: {CultureCode}", id, cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching advice ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var advice = await _adviceService.GetAdviceById(id, cultureCode);

            if (advice == null)
            {
                _logger.LogWarning("Advice with ID: {Id} not found", id);
                return StatusCode(StatusCodes.Status404NotFound, "Поради не знайдено.");
            }

            _logger.LogInformation("Successfully retrieved advice with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, advice);
        }

        /// <summary>
        /// Creates a new advice.
        /// </summary>
        /// <param name="adviceDto">The data for the new advice.</param>
        /// <returns>The newly created advice.</returns>
        /// <response code="201">Returns the newly created advice.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">If an error occurs while trying to create the advice.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdateAdviceDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdvice([FromBody] CreateAndUpdateAdviceDTO adviceDto)
        {
            _logger.LogInformation("Creating a new advice: {@Advice}", adviceDto);

            var newAdvice = await _adviceService.CreateAdvice(adviceDto);

            _logger.LogInformation("Successfully created new advice.");
            return StatusCode(StatusCodes.Status201Created, newAdvice);
        }

        /// <summary>
        /// Updates the details of a specific advice.
        /// </summary>
        /// <param name="id">The unique identifier of the advice to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the advice.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the advice details.</response>
        /// <returns>The updated details of the advice.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdvice(int id, [FromBody] JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc)
        {
            _logger.LogInformation("Updating advice with ID: {Id}", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Invalid request for updating advice with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Недійсний запит.");
            }

            var resultAdvice = await _adviceService.UpdateAdvice(id, patchDoc);

            _logger.LogInformation("Successfully updated advice with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, resultAdvice);
        }

        /// <summary>
        /// Deletes a specific advice.
        /// </summary>
        /// <param name="id">The unique identifier of the advice to be deleted.</param>
        /// <response code="204">Successful request. The advice has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">An internal server error occurred while trying to delete the advice.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdvice(int id)
        {
            _logger.LogInformation("Deleting advice with ID: {Id}", id);

            await _adviceService.DeleteAdvice(id);

            _logger.LogInformation("Successfully deleted advice with ID: {Id}", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
