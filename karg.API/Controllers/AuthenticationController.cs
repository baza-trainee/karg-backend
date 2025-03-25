using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, IEmailService emailService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
            _logger = logger;
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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            _logger.LogInformation("Attempting to log in with email: {Email}", loginDto.Email);

            var authResult = await _authenticationService.Authenticate(loginDto);

            if (authResult.Status == 0)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
                return StatusCode(StatusCodes.Status400BadRequest, authResult);
            }

            _logger.LogInformation("Successfully logged in with email: {Email}", loginDto.Email);
            return StatusCode(StatusCodes.Status200OK, authResult);
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
            _logger.LogInformation("Attempting to reset password with token: {Token}", credentials.Token);

            var resetResult = await _authenticationService.ResetPassword(credentials);

            if (resetResult.Status == 0)
            {
                _logger.LogWarning("Failed password reset attempt with token: {Token}", credentials.Token);
                return StatusCode(StatusCodes.Status400BadRequest, resetResult);
            }

            _logger.LogInformation("Successfully reset password with token: {Token}", credentials.Token);
            return StatusCode(StatusCodes.Status200OK, resetResult);
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
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmailDTO emailDto)
        {
            _logger.LogInformation("Attempting to send password reset email to: {Email}", emailDto.Email);

            var emailSendResult = await _emailService.SendPasswordResetEmail(emailDto.Email);

            if (emailSendResult.Status == 0)
            {
                _logger.LogWarning("Failed to send password reset email to: {Email}", emailDto.Email);
                return StatusCode(StatusCodes.Status400BadRequest, emailSendResult);
            }

            _logger.LogInformation("Successfully sent password reset email to: {Email}", emailDto.Email);
            return StatusCode(StatusCodes.Status200OK, emailSendResult);
        }
    }
}
