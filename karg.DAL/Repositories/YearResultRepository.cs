using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class YearResultRepository : BaseRepository<YearResult>, IYearResultRepository
    {
        public YearResultRepository(KargDbContext context) : base(context) { }

        public override async Task<List<YearResult>> GetAll()
        {
            return await _context.YearsResults
                .AsNoTracking()
                .Include(yearResult => yearResult.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .OrderByDescending(yearResult => yearResult.Id)
                .ToListAsync();
        }

        public override async Task<YearResult> GetById(int yearResultId)
        {
            return await _context.YearsResults
                .AsNoTracking()
                .Include(yearResult => yearResult.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(yearResult => yearResult.Id == yearResultId);
        }
    }
}
