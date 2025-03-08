using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class AdviceRepository : BaseRepository<Advice>, IAdviceRepository
    {
        public AdviceRepository(KargDbContext context) : base(context) { }

        public async Task<List<Advice>> GetAll(string nameSearch = null)
        {
            var advices = _context.Advices
                .AsNoTracking()
                .Include(advice => advice.Title).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(advice => advice.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .OrderByDescending(advice => advice.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                string lowerNameSearch = nameSearch.ToLower();
                advices = advices.Where(advice =>
                    advice.Title.Localizations.Any(localization =>
                        localization.Value.ToLower().Contains(lowerNameSearch)));
            }

            return await advices.ToListAsync();
        }

        public override async Task<Advice> GetById(int adviceId)
        {
            return await _context.Advices
                .AsNoTracking()
                .Include(advice => advice.Title).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(advice => advice.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(advice => advice.Id == adviceId);
        }
    }
}
