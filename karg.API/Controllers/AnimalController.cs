using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/animal")]
    public class AnimalController : Controller
    {
        private readonly IAnimalService _animalService;
        private readonly ICultureService _cultureService;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(IAnimalService animalService, ICultureService cultureService, ILogger<AnimalController> logger)
        {
            _animalService = animalService;
            _cultureService = cultureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of animals filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of animals.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific Animals.</param>
        /// <response code="200">Successful request. Returns a list of animals with the total number of pages.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of animals.</response>
        /// <returns>List of animals with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAnimals([FromQuery] AnimalsFilterDTO filter, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching all animals with filter: {@Filter} and culture: {CultureCode}", filter, cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching animals.");
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var paginatedAnimals = await _animalService.GetAnimals(filter, cultureCode);
            _logger.LogInformation("Successfully retrieved {Count} animals", paginatedAnimals.TotalItems);

            return StatusCode(StatusCodes.Status200OK, paginatedAnimals);
        }

        /// <summary>
        /// Gets the details of a specific animal by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the animal.</param>
        /// <param name="cultureCode">Optional. The culture code for language-specific details.</param>
        /// <response code="200">Successful request. Returns the details of the specified animal.</response>
        /// <response code="400">Invalid request parameters provided.</response>
        /// <response code="404">No animal found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the animal details.</response>
        /// <returns>The details of the specified animal.</returns>
        [HttpGet("getbyid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnimalById(int id, string cultureCode = "ua")
        {
            _logger.LogInformation("Fetching animal with ID: {Id} and culture: {CultureCode}", id, cultureCode);

            var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

            if (!ModelState.IsValid || !isValidCultureCode)
            {
                _logger.LogWarning("Invalid request parameters for fetching animal ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Надано недійсні параметри запиту.");
            }

            var animal = await _animalService.GetAnimalById(id, cultureCode);

            if (animal == null)
            {
                _logger.LogWarning("Animal with ID: {Id} not found", id);
                return StatusCode(StatusCodes.Status404NotFound, "Тварина не знайдена.");
            }
            _logger.LogInformation("Successfully retrieved animal with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, animal);
        }

        /// <summary>
        /// Creates a new animal.
        /// </summary>
        /// <param name="animalDto">The data for the new animal.</param>
        /// <returns>The newly created animal.</returns>
        /// <response code="201">Returns the newly created animal.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">If an error occurs while trying to create the animal.</response>
        [HttpPost("add")]
        [Authorize]
        [ProducesResponseType(typeof(CreateAndUpdateAnimalDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAndUpdateAnimalDTO animalDto)
        {
            _logger.LogInformation("Creating a new animal: {@AnimalDto}", animalDto);

            var newAnimal = await _animalService.CreateAnimal(animalDto);

            _logger.LogInformation("Successfully created new animal.");
            return StatusCode(StatusCodes.Status201Created, newAnimal);
        }

        /// <summary>
        /// Updates the details of a specific animal.
        /// </summary>
        /// <param name="id">The unique identifier of the animal to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the animal.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the animal details.</response>
        /// <returns>The updated details of the animal.</returns>
        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc)
        {
            _logger.LogInformation("Updating animal with ID: {Id}", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Invalid request for updating animal with ID: {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, "Недійсний запит.");
            }

            var resultAnimal = await _animalService.UpdateAnimal(id, patchDoc);

            _logger.LogInformation("Successfully updated animal with ID: {Id}", id);
            return StatusCode(StatusCodes.Status200OK, resultAnimal);
        }

        /// <summary>
        /// Deletes a specific animal.
        /// </summary>
        /// <param name="id">The unique identifier of the animal to be deleted.</param>
        /// <response code="204">Successful request. The animal has been deleted.</response>
        /// <response code="401">Unauthorized. The request requires user authentication.</response>
        /// <response code="403">Forbidden. The user does not have the necessary permissions to perform this action.</response>
        /// <response code="500">An internal server error occurred while trying to delete the animal.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            _logger.LogInformation("Deleting animal with ID: {Id}", id);

            await _animalService.DeleteAnimal(id);

            _logger.LogInformation("Successfully deleted animal with ID: {Id}", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}