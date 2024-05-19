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
        Task<PaginatedAnimalsDTO> GetAnimals(AnimalsFilterDTO filter, string cultureCode);
        Task CreateAnimal(CreateAnimalDTO animalDto);
        Task<AnimalDTO> UpdateAnimal(int id, JsonPatchDocument<AnimalDTO> patchDoc);
        Task<AnimalDTO> GetAnimalById(int id, string cultureCode);
        Task DeleteAnimal(int id);
    }
}
