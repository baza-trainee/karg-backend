using karg.BLL.DTO.Animals;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Animals
{
    public interface IAnimalService
    {
        Task<PaginatedAllAnimalsDTO> GetAnimals(AnimalsFilterDTO filter, string cultureCode);
        Task CreateAnimal(CreateAndUpdateAnimalDTO animalDto);
        Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int animalId, JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc);
        Task<AnimalDTO> GetAnimalById(int animalId, string cultureCode);
        Task DeleteAnimal(int animalId);
    }
}
