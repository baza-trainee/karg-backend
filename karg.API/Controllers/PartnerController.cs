﻿using karg.BLL.DTO.Partners;
using karg.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        /// <summary>
        /// Gets the details of a specific partner by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the partner.</param>
        /// <response code="200">Successful request. Returns the details of the specified partner.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No partner found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the partner details.</response>
        /// <returns>The details of the specified partner.</returns>
        [HttpGet("getbyid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPartnerById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var partner = await _partnerService.GetPartnerById(id);

                if (partner == null)
                {
                    return NotFound("Partner not found.");
                }

                return Ok(partner);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new partner.
        /// </summary>
        /// <param name="partnerDto">The data for the new partner.</param>
        /// <returns>The newly created partner.</returns>
        /// <response code="201">Returns the newly created partner.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">If an error occurs while trying to create the partner.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdatePartnerDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePartner([FromBody] CreateAndUpdatePartnerDTO partnerDto)
        {
            try
            {
                await _partnerService.CreatePartner(partnerDto);

                return Created("CreatePartner", partnerDto);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Updates the details of a specific partner.
        /// </summary>
        /// <param name="id">The unique identifier of the partner to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the partner.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the partner details.</response>
        /// <returns>The updated details of the partner.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartner(int id, [FromBody] JsonPatchDocument<CreateAndUpdatePartnerDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultPartner = await _partnerService.UpdatePartner(id, patchDoc);

                return Ok(resultPartner);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Deletes a specific partner.
        /// </summary>
        /// <param name="id">The unique identifier of the partner to be deleted.</param>
        /// <response code="204">Successful request. The partner has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="500">An internal server error occurred while trying to delete the partner.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePartner(int id)
        {
            try
            {
                await _partnerService.DeletePartner(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
