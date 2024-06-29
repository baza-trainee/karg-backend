using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        /// <param name="cultureCode">Optional. The culture code for language-specific FAQs.</param>
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
        /// Gets the details of a specific FAQ by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details.</param>
        /// <response code="200">Successful request. Returns the details of the specified FAQ.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No FAQ found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the FAQ details.</response>
        /// <returns>The details of the specified FAQ.</returns>
        [HttpGet("getbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQById(int id, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!ModelState.IsValid || !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var faq = await _faqService.GetFAQById(id, cultureCode);

                if (faq == null)
                {
                    return NotFound("FAQ not found.");
                }

                return Ok(faq);
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

        /// <summary>
        /// Updates the details of a specific FAQ.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the FAQ.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the FAQ details.</response>
        /// <returns>The updated details of the FAQ.</returns>
        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultFAQ = await _faqService.UpdateFAQ(id, patchDoc);

                return Ok(resultFAQ);
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
