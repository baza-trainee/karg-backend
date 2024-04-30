using karg.BLL.Interfaces.Partners;
using karg.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/partner")]
    public class PartnerController : Controller
    {
        private IPartnerService _partnerService {  get; set; }

        public PartnerController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllPartners()
        {
            try
            {
                var partners = await _partnerService.GetPartners();

                if (partners.Count() == 0)
                {
                    return NotFound("Partners not found.");
                }

                return Ok(partners);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
