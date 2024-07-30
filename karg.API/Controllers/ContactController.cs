using karg.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/contact")]
    public class ContactController : Controller
    {
        private IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Gets a list of contacts.
        /// </summary>
        /// <response code="200">Successful request. Returns a list of contacts.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No contacts found.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of contacts.</response>
        /// <returns>List of contacts.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var contacts = await _contactService.GetContacts();

                if (contacts.Count == 0)
                {
                    return NotFound("Contacts not found.");
                }

                return Ok(contacts);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Gets the details of a specific contact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <response code="200">Successful request. Returns the details of the specified contact.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="404">No contact found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the contact details.</response>
        /// <returns>The details of the specified contact.</returns>
        [HttpGet("getbyid")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContactById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                {
                    return NotFound("Contact not found.");
                }

                return Ok(contact);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}