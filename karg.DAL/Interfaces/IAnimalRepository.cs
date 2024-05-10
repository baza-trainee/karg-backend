using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAnimals(string categoryFilter = null, string nameSearch = null);
        Task<int> AddAnimal(Animal animal);
        Task<Animal> GetAnimal(int animalId);
        Task<Animal> UpdateAnimal(Animal existingAnimal, Animal updatedAnimal);
        Task Delete(Animal animal);
    }
}
