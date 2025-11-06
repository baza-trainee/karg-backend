using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class LocalizationSetRepository : BaseRepository<LocalizationSet>, ILocalizationSetRepository
    {
        public LocalizationSetRepository(KargDbContext context) : base(context) { }

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
