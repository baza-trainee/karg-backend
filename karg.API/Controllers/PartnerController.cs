using karg.BLL.Interfaces.Partners;
using karg.BLL.Services;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Gets a list of partners
        /// </summary>
        /// <remarks>
        /// This endpoint allows anonymous access.
        /// </remarks>
        /// <response code="200">Successful request. Returns a list of partners.</response>
        /// <response code="404">Partners not found. No partners are available in the system.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of partners.</response>
        /// <returns>List of partners.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
