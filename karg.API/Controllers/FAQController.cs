using karg.BLL.DTO.FAQs;
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

        public FAQController(IFAQService faqService, ICultureService cultureService)
        {
            _faqService = faqService;
            _cultureService = cultureService;
        }

        /// <summary>
        /// Gets a list of all Frequently Asked Questions (FAQs).
        /// </summary>
        /// <param name="cultureCode">Optional. The culture code for language-specific FAQs. Default is "ua".</param>
        /// <response code="200">Successful request. Returns a list of all FAQs.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No FAQs found for the specified culture code.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of FAQs.</response>
        /// <returns>List of all FAQs.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Creates a new FAQ.
        /// </summary>
        /// <param name="faqDto">The data for the new FAQ.</param>
        /// <returns>The newly created FAQ.</returns>
        /// <response code="201">Returns the newly created FAQ.</response>
        /// <response code="500">If an error occurs while trying to create the FAQ.</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(CreateAndUpdateFAQDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateAndUpdateFAQDTO faqDto)
        {
            try
            {
                await _faqService.CreateFAQ(faqDto);

                return Created("CreateFAQ", faqDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
