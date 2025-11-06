using karg.BLL.DTO.Rescuers;
using karg.BLL.Interfaces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/rescuer")]
    public class RescuerController : Controller
    {
        private readonly IRescuerService _rescuerService;
        private readonly ILogger<RescuerController> _logger;

        public RescuerController(IRescuerService rescuerService, ILogger<RescuerController> logger)
        {
            _rescuerService = rescuerService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all rescuers registered in the system.
        /// </summary>
        /// <response code="200">Successful request. Returns a list of rescuers.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of rescuers.</response>
        /// <returns>List of rescuers.</returns>
        [HttpGet("getall")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRescuers([FromQuery] RescuersFilterDTO filter)
        {
            _logger.LogInformation("Fetching all rescuers with filter: {@Filter}", JsonSerializer.Serialize(filter));

            var paginatedRescuers = await _rescuerService.GetRescuers(filter);

            _logger.LogInformation("Successfully retrieved {Count} rescuers", paginatedRescuers.Items.Count);
            return StatusCode(StatusCodes.Status200OK, paginatedRescuers);
        }

        /// <summary>
        /// Gets the details of a specific rescuer by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rescuer.</param>
        /// <response code="200">Successful request. Returns the details of the specified rescuer.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the required permissions to view this rescuer.</response>
        /// <response code="404">No rescuer found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the rescuer details.</response>
        /// <returns>The details of the specified rescuer.</returns>
        [HttpGet("getbyid")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRescuerById(int id)
        {
            _logger.LogInformation("Fetching rescuer with ID: {RescuerId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request parameters for rescuer ID: {RescuerId}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var rescuer = await _rescuerService.GetRescuerById(id);

            if (rescuer == null)
            {
                _logger.LogWarning("Rescuer not found with ID: {RescuerId}", id);
                return StatusCode(StatusCodes.Status404NotFound, "Працівника не знайдено.");
            }

            return StatusCode(StatusCodes.Status200OK, rescuer);
        }

        /// <summary>
        /// Creates a new rescuer.
        /// </summary>
        /// <param name="rescuerDto">The data for the new rescuer.</param>
        /// <returns>The newly created rescuer.</returns>
        /// <response code="201">Returns the newly created rescuer.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the required role to perform this action.</response>
        /// <response code="409">Conflict. A rescuer with the same email already exists.</response>
        /// <response code="500">If an error occurs while trying to create the rescuer.</response>
        [HttpPost("add")]
        [Authorize(Policy = "Director")]
        [ProducesResponseType(typeof(CreateAndUpdateRescuerDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRescuer([FromBody] CreateAndUpdateRescuerDTO rescuerDto)
        {
            _logger.LogInformation("Creating a new rescuer: {@Rescuer}", JsonSerializer.Serialize(rescuerDto));

            var existingRescuer = await _rescuerService.GetRescuerByEmail(rescuerDto.Email);

            if (existingRescuer != null)
            {
                _logger.LogWarning("Rescuer creation failed: Email {Email} already exists", rescuerDto.Email);
                return StatusCode(StatusCodes.Status409Conflict, "Працівник з такою електронною поштою вже існує.");
            }

            var newRescuer = await _rescuerService.CreateRescuer(rescuerDto);

            _logger.LogInformation("Successfully created new rescuer.");
            return StatusCode(StatusCodes.Status201Created, newRescuer);
        }

        /// <summary>
        /// Updates the details of a specific rescuer.
        /// </summary>
        /// <param name="id">The unique identifier of the rescuer to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the rescuer.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the required permissions to update this rescuer.</response>
        /// <response code="409">Conflict. A rescuer with the same email already exists.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the rescuer details.</response>
        /// <returns>The updated details of the rescuer.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRescuer(int id, [FromBody] JsonPatchDocument<CreateAndUpdateRescuerDTO> patchDoc)
        {
            _logger.LogInformation("Updating rescuer with ID: {Id}", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Invalid request for updating rescuer with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Недійсний запит.");
            }

            var emailOperation = patchDoc.Operations.FirstOrDefault(op => op.path.Equals("/email", StringComparison.OrdinalIgnoreCase));

            if (emailOperation != null)
            {
                var rescuerWithSameEmail = await _rescuerService.GetRescuerByEmail(emailOperation.value?.ToString());
                if (rescuerWithSameEmail != null)
                {
                    _logger.LogWarning("Update failed: Email {Email} already exists", emailOperation.value);
                    return StatusCode(StatusCodes.Status409Conflict, "Працівник з такою електронною поштою вже існує.");
                }
            }

            var resultRescuer = await _rescuerService.UpdateRescuer(id, patchDoc);

            _logger.LogInformation("Successfully updated rescuer with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, resultRescuer);
        }

        /// <summary>
        /// Deletes a specific rescuer.
        /// </summary>
        /// <param name="id">The unique identifier of the rescuer to be deleted.</param>
        /// <response code="204">Successful request. The rescuer has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the required role to perform this action.</response>
        /// <response code="500">An internal server error occurred while trying to delete the rescuer.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRescuer(int id)
        {
            _logger.LogInformation("Deleting rescuer with ID: {Id}", id);

            await _rescuerService.DeleteRescuer(id);

            _logger.LogInformation("Successfully deleted rescuer with ID: {Id}", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}