using karg.BLL.DTO.Authentication;
using karg.BLL.DTO.Rescuers;
using karg.BLL.Interfaces.Rescuers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/rescuer")]
    public class RescuerController : Controller
    {
        private IRescuerService _rescuerService;

        public RescuerController(IRescuerService rescuerService)
        {
            _rescuerService = rescuerService;
        }

        /// <summary>
        /// Gets all rescuers registered in the system.
        /// </summary>
        /// <response code="200">Successful request. Returns a list of rescuers.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="404">Rescuers not found. No rescuers are available in the system.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of rescuers.</response>
        /// <returns>List of rescuers.</returns>
        [HttpGet("getall")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRescuers()
        {
            try
            {
                var rescuers = await _rescuerService.GetRescuers();

                if (rescuers.Count() == 0)
                {
                    return NotFound("Rescuers not found.");
                }

                return Ok(rescuers);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Gets the details of a specific rescuer by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rescuer.</param>
        /// <response code="200">Successful request. Returns the details of the specified rescuer.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="404">No rescuer found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the rescuer details.</response>
        /// <returns>The details of the specified rescuer.</returns>
        [HttpGet("getbyid")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdviceById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var rescuer = await _rescuerService.GetRescuerById(id);

                if (rescuer == null)
                {
                    return NotFound("Rescuer not found.");
                }

                return Ok(rescuer);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new rescuer.
        /// </summary>
        /// <param name="rescuerDto">The data for the new rescuer.</param>
        /// <returns>The newly created rescuer.</returns>
        /// <response code="201">Returns the newly created rescuer.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the required role to perform this action.</response>
        /// <response code="500">If an error occurs while trying to create the rescuer.</response>
        [HttpPost("add")]
        [Authorize(Policy = "Director")]
        [ProducesResponseType(typeof(CreateAndUpdateRescuerDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRescuer([FromBody] CreateAndUpdateRescuerDTO rescuerDto)
        {
            try
            {
                await _rescuerService.CreateRescuer(rescuerDto);

                return Created("CreateRescuer", rescuerDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Updates the details of a specific rescuer.
        /// </summary>
        /// <param name="id">The unique identifier of the rescuer to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the rescuer.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the rescuer details.</response>
        /// <returns>The updated details of the rescuer.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRescuer(int id, [FromBody] JsonPatchDocument<CreateAndUpdateRescuerDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultRescuer = await _rescuerService.UpdateRescuer(id, patchDoc);

                return Ok(resultRescuer);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
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
        [Authorize(Policy = "Director")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRescuer(int id)
        {
            try
            {
                await _rescuerService.DeleteRescuer(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}