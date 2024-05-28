﻿using karg.BLL.Interfaces.FAQs;
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
        /// Deletes a specific FAQ.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ to be deleted.</param>
        /// <response code="204">Successful request. The FAQ has been deleted.</response>
        /// <response code="500">An internal server error occurred while trying to delete the FAQ.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                await _faqService.DeleteFAQ(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
