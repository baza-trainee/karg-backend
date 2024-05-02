using karg.BLL.DTO.Animals;
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
        Task CreateAnimal(CreateAnimalDTO animalDto);
    }
}
