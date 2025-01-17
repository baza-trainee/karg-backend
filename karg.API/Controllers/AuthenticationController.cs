using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/authentication")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IEmailService _emailService;

        public AuthenticationController(IAuthenticationService authenticationService, IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
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
                var authResult = await _authenticationService.Authenticate(loginDto);

                if (authResult.Status == 0)
                {
                    return BadRequest(authResult);
                }
                
                return Ok(authResult);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Resets the password for the specified rescuer based on their email.
        /// </summary>
        /// <param name="credentials">Object containing the token and new password.</param>
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
                var resetResult = await _authenticationService.ResetPassword(credentials);

                if (resetResult.Status == 0)
                {
                    return BadRequest(resetResult);
                }

                return Ok(resetResult);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Sends an email to the user with a reset password token.
        /// </summary>
        /// <param name="emailDto">Object containing the user's email address.</param>
        /// <response code="200">Email sent successfully.</response>
        /// <response code="400">Bad Request. Invalid email address.</response>
        /// <response code="500">Internal Server Error. Failed to send the email.</response>
        /// <returns>Status of the email sending operation.</returns>
        [HttpPost("sendresetpasswordemail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody]SendResetPasswordEmailDTO emailDto)
        {
            try
            {
                var emailSendResult = await _emailService.SendPasswordResetEmail(emailDto.Email);

                if (emailSendResult.Status == 0)
                {
                    return BadRequest(emailSendResult);
                }

                return Ok(emailSendResult);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
