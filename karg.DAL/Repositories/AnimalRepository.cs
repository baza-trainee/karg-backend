using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly KargDbContext _context;

        public AnimalRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAnimals(string categoryFilter = null, string nameSearch = null)
        {
            var animals = _context.Animals
                .AsNoTracking()
                .Include(animal => animal.Name).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Story).ThenInclude(localizationSet => localizationSet.Localizations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryFilter))
            {
                var category = Enum.Parse<AnimalCategory>(categoryFilter, true);
                animals = animals.Where(animal => animal.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                animals = animals.Where(animal =>
                    animal.Name.Localizations.Any(localization => string.Equals(localization.Value,nameSearch, StringComparison.OrdinalIgnoreCase)));
            }

            return await animals.ToListAsync();
        }

        public async Task<int> AddAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return animal.Id;
        }

        public async Task<Animal> GetAnimal(int animalId)
        {
            return await _context.Animals
                .AsNoTracking()
                .Include(animal => animal.Name).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Story).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(animal => animal.Id == animalId);
        }

        public async Task UpdateAnimal(Animal updatedAnimal)
        {
            var existingAnimal = await _context.Animals.FindAsync(updatedAnimal.Id);

            _context.Entry(existingAnimal).CurrentValues.SetValues(updatedAnimal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnimal(Animal animal)
        {
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
        }
    }
}
