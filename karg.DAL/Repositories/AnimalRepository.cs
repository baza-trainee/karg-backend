using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class AnimalRepository : BaseRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(KargDbContext context) : base(context) { }

        public async Task<List<Animal>> GetAll(string categoryFilter = null, string nameSearch = null)
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
                nameSearch = nameSearch.ToLower();
                animals = animals.Where(animal =>
                    animal.Name.Localizations.Any(localization => localization.Value.ToLower() == nameSearch));
            }

            return await animals.ToListAsync();
        }

        public override async Task<Animal> GetById(int animalId)
        {
            return await _context.Animals
                .AsNoTracking()
                .Include(animal => animal.Name).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Story).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(animal => animal.Id == animalId);
        }
    }
}
