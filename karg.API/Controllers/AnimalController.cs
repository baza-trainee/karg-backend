using karg.BLL.DTO.Animals;
using karg.BLL.Interfaces.Animals;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/animal")]
    public class AnimalController : Controller
    {
        private IAnimalService _animalService;
        private ICultureService _cultureService;

        public AnimalController(IAnimalService animalService, ICultureService cultureService)
        {
            _animalService = animalService;
            _cultureService = cultureService;
        }

        /// <summary>
        /// Gets a list of animals filtered by the specified criteria.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of animals.</param>
        /// <response code="200">Successful request. Returns a list of animals with the total number of pages.</response>
        /// <response code="404">No animals found based on the specified filters.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of animals.</response>
        /// <returns>List of animals with total number of pages.</returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAnimals([FromQuery]AnimalsFilterDTO filter, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var paginatedAnimals = await _animalService.GetAnimals(filter, cultureCode);

                if (paginatedAnimals.Animals.Count == 0)
                {
                    return NotFound("Animals not found.");
                }

                return Ok(paginatedAnimals);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Gets the details of a specific animal by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the animal.</param>
        /// <response code="200">Successful request. Returns the details of the specified animal.</response>
        /// <response code="404">No animal found with the specified identifier.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the animal details.</response>
        /// <returns>The details of the specified animal.</returns>
        [HttpGet("getbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnimalById(int id, string cultureCode = "ua")
        {
            try
            {
                var isValidCultureCode = await _cultureService.IsCultureCodeInDatabase(cultureCode);

                if (ModelState.IsValid && !isValidCultureCode)
                {
                    return BadRequest("Invalid request parameters provided.");
                }

                var animal = await _animalService.GetAnimalById(id, cultureCode);

                if (animal == null)
                {
                    return NotFound("Animal not found.");
                }

                return Ok(animal);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Creates a new animal.
        /// </summary>
        /// <param name="animalDto">The data for the new animal.</param>
        /// <returns>The newly created animal.</returns>
        /// <response code="201">Returns the newly created animal.</response>
        /// <response code="500">If an error occurs while trying to create the animal.</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(CreateAnimalDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalDTO animalDto)
        {
            try
            {
                await _animalService.CreateAnimal(animalDto);

                return Created("CreateAnimal", animalDto);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Updates the details of a specific animal.
        /// </summary>
        /// <param name="id">The unique identifier of the animal to be updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates to apply.</param>
        /// <response code="200">Successful request. Returns the updated details of the animal.</response>
        /// <response code="400">Bad request. If the JSON Patch document is null.</response>
        /// <response code="500">Internal server error. An error occurred while trying to update the animal details.</response>
        /// <returns>The updated details of the animal.</returns>
        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] JsonPatchDocument<AnimalDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var resultAnimal = await _animalService.UpdateAnimal(id, patchDoc);

                return Ok(resultAnimal);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Deletes a specific animal.
        /// </summary>
        /// <param name="id">The unique identifier of the animal to be deleted.</param>
        /// <response code="204">Successful request. The animal has been deleted.</response>
        /// <response code="500">An internal server error occurred while trying to delete the animal.</response>
        /// <returns>No content.</returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                await _animalService.DeleteAnimal(id);

                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}