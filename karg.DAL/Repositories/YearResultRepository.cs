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
    public class YearResultRepository : IYearResultRepository
    {
        private readonly KargDbContext _context;

        public YearResultRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<YearResult>> GetYearsResults()
        {
            return await _context.YearsResults
                .AsNoTracking()
                .Include(advice => advice.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .ToListAsync();
        }
    }
}
