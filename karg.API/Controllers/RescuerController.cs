using karg.BLL.DTO;
using karg.BLL.Interfaces;
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

        [HttpPost("resetpassword")]
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
    }
}