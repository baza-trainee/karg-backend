using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Repositories
{
    public class LocalizationSetRepository : ILocalizationSetRepository
    {
        private readonly KargDbContext _context;

        public LocalizationSetRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddLocalizationSet(LocalizationSet localizationSet)
        {
            _context.LocalizationSets.Add(localizationSet);
            await _context.SaveChangesAsync();

            return localizationSet.Id;
        }

        public async Task<List<LocalizationSet>> GetLocalizationSets(List<int> localizationSetIds)
        {
            return await _context.LocalizationSets.Where(localizationSet => localizationSetIds.Contains(localizationSet.Id)).ToListAsync();
        }

        public async Task DeleteLocalizationSets(List<LocalizationSet> localizationSets)
        {
            _context.LocalizationSets.RemoveRange(localizationSets);
            await _context.SaveChangesAsync();
        }
    }
}
