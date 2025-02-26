using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class AdviceRepository : BaseRepository<Advice>, IAdviceRepository
    {
        public AdviceRepository(KargDbContext context) : base(context) { }

        public override async Task<List<Advice>> GetAll()
        {
            return await _context.Advices
                .AsNoTracking()
                .Include(advice => advice.Title).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(advice => advice.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .OrderByDescending(advice => advice.Id)
                .ToListAsync();
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
