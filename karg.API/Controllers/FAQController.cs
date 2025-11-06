using karg.BLL.DTO.FAQs;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/faq")]
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;
        private readonly ICultureService _cultureService;
        private readonly ILogger<FAQController> _logger;

        public FAQController(IFAQService faqService, ICultureService cultureService, ILogger<FAQController> logger)
        {
            _faqService = faqService;
            _cultureService = cultureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all Frequently Asked Questions (FAQs).
        /// </summary>
        /// <param name="cultureCode">Optional. The culture code for language-specific FAQs.</param>
        /// <response code="200">Successful request. Returns a list of all FAQs.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of FAQs.</response>
        /// <returns>List of all FAQs.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFAQs([FromQuery] FAQsFilterDTO filter, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching all FAQs with filter: {@Filter} and culture: {CultureCode}", JsonSerializer.Serialize(filter), cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching all FAQs");
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var paginatedFAQs = await _faqService.GetFAQs(filter, cultureCode);

            _logger.LogInformation("Successfully retrieved {Count} FAQs", paginatedFAQs.Items.Count);
            return StatusCode(StatusCodes.Status200OK, paginatedFAQs);
        }

        /// <summary>
        /// Gets the details of a specific FAQ by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details.</param>
        /// <response code="200">Successful request. Returns the details of the specified FAQ.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No FAQ found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the FAQ details.</response>
        /// <returns>The details of the specified FAQ.</returns>
        [HttpGet("getbyid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQById(int id, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching FAQ with ID: {Id} and culture code: {CultureCode}", id, cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid parameters for GetFAQById with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var faq = await _faqService.GetFAQById(id, cultureCode);

            if (faq == null)
            {
                _logger.LogWarning("FAQ with ID: {Id} not found", id);
                return StatusCode(StatusCodes.Status404NotFound, "FAQ не знайдено.");
            }

            _logger.LogInformation("Successfully retrieved FAQ with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, faq);
        }

        /// <summary>
        /// Creates a new FAQ.
        /// </summary>
        /// <param name="faqDto">The data for the new FAQ.</param>
        /// <returns>The newly created FAQ.</returns>
        /// <response code="201">Returns the newly created FAQ.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">If an error occurs while trying to create the FAQ.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdateFAQDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateAndUpdateFAQDTO faqDto)
        {
            _logger.LogInformation("Creating a new FAQ: {@FAQ}", JsonSerializer.Serialize(faqDto));

            await _faqService.CreateFAQ(faqDto);

            _logger.LogInformation("Successfully created a new FAQ");
            return StatusCode(StatusCodes.Status201Created, faqDto);
        }

        /// <summary>
        /// Updates the details of a specific FAQ.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the FAQ.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the FAQ details.</response>
        /// <returns>The updated details of the FAQ.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc)
        {
            _logger.LogInformation("Updating FAQ with ID: {Id}", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Invalid request for updating FAQ with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Недійсний запит.");
            }

            var resultFAQ = await _faqService.UpdateFAQ(id, patchDoc);

            _logger.LogInformation("Successfully updated FAQ with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, resultFAQ);
        }

        /// <summary>
        /// Deletes a specific FAQ.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ to be deleted.</param>
        /// <response code="204">Successful request. The FAQ has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">An internal server error occurred while trying to delete the FAQ.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            _logger.LogInformation("Deleting FAQ with ID: {Id}", id);

            await _faqService.DeleteFAQ(id);

            _logger.LogInformation("Successfully deleted FAQ with ID: {Id}", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
