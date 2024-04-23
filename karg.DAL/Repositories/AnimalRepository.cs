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

        public async Task<List<Animal>> GetAnimals(int page, int pageSize, string categoryFilter = null, string nameSearch = null)
        {
            var animalsQuery = _context.Animals.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(categoryFilter))
            {
                var category = Enum.Parse<AnimalCategory>(categoryFilter, true);
                animalsQuery = animalsQuery.Where(animal => animal.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                animalsQuery = animalsQuery.Where(animal => animal.Name.ToLower().Contains(nameSearch.ToLower()));
            }

            var animals = await animalsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return animals;
        }
    }
}
