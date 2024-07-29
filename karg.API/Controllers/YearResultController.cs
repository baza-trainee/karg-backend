﻿using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/yearresult")]
    public class YearResultController : Controller
    {
        private IYearResultService _yearResultService;
        private ICultureService _cultureService;

        public YearResultController(IYearResultService yearResultService, ICultureService cultureService)
        {
            _yearResultService = yearResultService;
            _cultureService = cultureService;
        }

        /// <summary>
        /// Gets a list of years results filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of years results.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific years results.</param>
        /// <response code="200">Successful request. Returns a list of years results with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No years results found based on the specified filters.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of years results.</response>
        /// <returns>List of years results with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllYearsResults([FromQuery] YearsResultsFilterDTO filter, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!ModelState.IsValid || !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var paginatedYearsResults = await _yearResultService.GetYearsResults(filter, cultureCode);

                if (paginatedYearsResults.YearsResults.Count == 0)
                {
                    return NotFound("Years results not found.");
                }

                return Ok(paginatedYearsResults);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Gets the year result details by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the year result.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details. Default is "ua".</param>
        /// <response code="200">Successful request. Returns the details of the specified year result.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="404">No year result found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the year result details.</response>
        /// <returns>The details of the specified year result.</returns>
        [HttpGet("getbyid")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYearResultById(int id, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (!ModelState.IsValid || !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var yearResult = await _yearResultService.GetYearResultById(id, cultureCode);

                if (yearResult == null)
                {
                    return NotFound("Year result not found.");
                }

                return Ok(yearResult);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new year result.
        /// </summary>
        /// <param name="yearResultDto">The data for the new year result.</param>
        /// <returns>The newly created year result.</returns>
        /// <response code="201">Returns the newly created year result.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">If an error occurs while trying to create the year result.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdateYearResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateYearResult([FromBody] CreateAndUpdateYearResultDTO yearResultDto)
        {
            try
            {
                await _yearResultService.CreateYearResult(yearResultDto);

                return Created("CreateYearResult", yearResultDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Updates the details of a specific year result.
        /// </summary>
        /// <param name="id">The unique identifier of the year result to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the year result.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the year result details.</response>
        /// <returns>The updated details of the year result.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateYearResult(int id, [FromBody] JsonPatchDocument<CreateAndUpdateYearResultDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultYearResult = await _yearResultService.UpdateYearResult(id, patchDoc);

                return Ok(resultYearResult);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Deletes a specific year result.
        /// </summary>
        /// <param name="id">The unique identifier of the year result to be deleted.</param>
        /// <response code="204">Successful request. The year result has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">An internal server error occurred while trying to delete the year result.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteYearResult(int id)
        {
            try
            {
                await _yearResultService.DeleteYearResult(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
