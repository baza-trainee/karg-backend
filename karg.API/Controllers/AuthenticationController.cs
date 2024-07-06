using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var result = await _authenticationService.Authenticate(loginDto);

                if (result.Status == 0)
                {
                    return BadRequest(result.Message);
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
