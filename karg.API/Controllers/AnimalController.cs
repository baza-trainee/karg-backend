using karg.BLL.DTO;
using karg.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("karg/animal")]
    public class AnimalController : Controller
    {
        private IAnimalService _animalService {  get; set; }

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAnimals([FromQuery]AnimalsFilterDTO filter)
        {
            try
            {
                var animals = await _animalService.GetAnimals(filter);

                if(animals.Count() == 0) 
                {
                    return NotFound("Animals not found.");
                }

                return Ok(animals);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
