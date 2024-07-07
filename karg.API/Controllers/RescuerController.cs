using karg.BLL.DTO.Authentication;
using karg.BLL.DTO.Rescuers;
using karg.BLL.Interfaces.Rescuers;
using Microsoft.AspNetCore.Authorization;
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
        /// Resets the password for the specified rescuer based on their email.
        /// </summary>
        /// <param name="credentials">Object containing the rescuer's email and new password.</param>
        /// <response code="200">Successful password reset.</response>
        /// <response code="500">Internal server error. Failed to process the request.</response>
        /// <returns>Message indicating successful password reset.</returns>
        [HttpPost("resetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO credentials)
        {
            try
            {
                await _rescuerService.ResetPassword(credentials.Email, credentials.Password);

                return Ok("Password reset successfully.");
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