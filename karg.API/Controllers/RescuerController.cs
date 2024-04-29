using karg.BLL.DTO;
using karg.BLL.Interfaces;
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

        [HttpGet("getall")]
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