using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace karg.DAL.Repositories
{
    public class AnimalRepository : BaseRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(KargDbContext context) : base(context) { }

        public async Task<List<Animal>> GetAll(AnimalSortOrder sortOrder, string categoryFilter = null, string nameSearch = null)
        {
            var animals = _context.Animals
                .AsNoTracking()
                .Include(animal => animal.Name).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Story).ThenInclude(localizationSet => localizationSet.Localizations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryFilter) && Enum.TryParse<AnimalCategory>(categoryFilter, true, out var category))
            {
                animals = animals.Where(animal => animal.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                string lowerNameSearch = nameSearch.ToLower();
                animals = animals.Where(animal =>
                    animal.Name.Localizations.Any(localization =>
                        localization.Value.ToLower().Contains(lowerNameSearch)));
            }

            animals = sortOrder switch
            {
                AnimalSortOrder.Latest => animals.OrderByDescending(animal => animal.DateCreated),
                AnimalSortOrder.Oldest => animals.OrderBy(animal => animal.DateCreated),
                _ => animals
            };

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
