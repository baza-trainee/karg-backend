using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.Advices;
using karg.BLL.DTO.Advices;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/advice")]
    public class AdviceController : Controller
    {
        private IAdviceService _adviceService;
        private ICultureService _cultureService;

        public AdviceController(IAdviceService adviceService, ICultureService cultureService)
        {
            _adviceService = adviceService;
            _cultureService = cultureService;
        }

        /// <summary>
        /// Gets a list of advices filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of advices.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific advices.</param>
        /// <response code="200">Successful request. Returns a list of advices with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No advices found based on the specified filters.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of advices.</response>
        /// <returns>List of advices with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdvices([FromQuery] AdvicesFilterDTO filter, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var paginatedAdvices = await _adviceService.GetAdvices(filter, cultureCode);

                if (paginatedAdvices.Advices.Count == 0)
                {
                    return NotFound("Advices not found.");
                }

                return Ok(paginatedAdvices);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Gets the details of a specific advice by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the advice.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details.</param>
        /// <response code="200">Successful request. Returns the details of the specified advice.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the advice details.</response>
        /// <returns>The details of the specified advice.</returns>
        [HttpGet("getbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdviceById(int id, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var advice = await _adviceService.GetAdviceById(id, cultureCode);

                return Ok(advice);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new advice.
        /// </summary>
        /// <param name="adviceDto">The data for the new advice.</param>
        /// <returns>The newly created advice.</returns>
        /// <response code="201">Returns the newly created advice.</response>
        /// <response code="500">If an error occurs while trying to create the advice.</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(CreateAndUpdateAdviceDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdvice([FromBody] CreateAndUpdateAdviceDTO adviceDto)
        {
            try
            {
                await _adviceService.CreateAdvice(adviceDto);

                return Created("CreateAdvice", adviceDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Updates the details of a specific advice.
        /// </summary>
        /// <param name="id">The unique identifier of the advice to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the advice.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the advice details.</response>
        /// <returns>The updated details of the advice.</returns>
        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdvice(int id, [FromBody] JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultAdvice = await _adviceService.UpdateAdvice(id, patchDoc);

                return Ok(resultAdvice);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Deletes a specific advice.
        /// </summary>
        /// <param name="id">The unique identifier of the advice to be deleted.</param>
        /// <response code="204">Successful request. The advice has been deleted.</response>
        /// <response code="500">An internal server error occurred while trying to delete the advice.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdvice(int id)
        {
            try
            {
                await _adviceService.DeleteAdvice(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
