using karg.BLL.DTO.Animals;
using karg.BLL.Interfaces.Animals;
using karg.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/animal")]
    public class AnimalController : Controller
    {
        private IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAnimals([FromQuery]AnimalsFilterDTO filter)
        {
            try
            {
                var paginatedAnimals = await _animalService.GetAnimals(filter);

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
    }
}
