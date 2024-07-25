using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/authentication")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticates a user based on the provided credentials.
        /// </summary>
        /// <param name="loginDto">The credentials for login.</param>
        /// <returns>An authentication result indicating the status of the login attempt.</returns>
        /// <response code="200">The login was successful. Returns the authentication result.</response>
        /// <response code="400">Bad Request. The request parameters are invalid.</response>
        /// <response code="500">Internal Server Error. An error occurred during the login process.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDto)
        {
            try
            {
                var result = await _authenticationService.Authenticate(loginDto);

                if (result.Status == 0)
                {
                    return BadRequest(result);
                }
                
                return Ok(result);
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
        /// <response code="400">Bad Request. The request parameters are invalid.</response>
        /// <response code="500">Internal server error. Failed to process the request.</response>
        /// <returns>Message indicating successful password reset.</returns>
        [HttpPost("resetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO credentials)
        {
            try
            {
                var result = await _authenticationService.ResetPassword(credentials);

                if (result.Status == 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
