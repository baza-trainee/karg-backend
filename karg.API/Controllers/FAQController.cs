using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/faq")]
    public class FAQController : Controller
    {
        private IFAQService _faqService;
        private ICultureService _cultureService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        /// <summary>
        /// Gets a list of all Frequently Asked Questions (FAQs).
        /// </summary>
        /// <response code="200">Successful request. Returns a list of all FAQs.</response>
        /// <response code="404">No FAQs found.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of FAQs.</response>
        /// <returns>List of all FAQs.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFAQs(string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var faqs = await _faqService.GetFAQs(cultureCode);

                if (faqs.Count() == 0)
                {
                    return NotFound("FAQs not found.");
                }

                return Ok(faqs);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
