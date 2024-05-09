using karg.BLL.DTO.Animals;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Animals
{
    public interface IAnimalService
    {
        Task<PaginatedAnimalsDTO> GetAnimals(AnimalsFilterDTO filter);
        Task CreateAnimal(CreateAndUpdateAnimalDTO animalDto);
        Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int id, CreateAndUpdateAnimalDTO animalDto);
        Task<AnimalDTO> GetAnimalById(int id);
    }
}
