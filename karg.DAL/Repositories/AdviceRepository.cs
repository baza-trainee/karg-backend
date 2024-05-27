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
    public class AdviceRepository : IAdviceRepository
    {
        private readonly KargDbContext _context;

        public AdviceRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Advice>> GetAdvices()
        {
            return await _context.Advices
                .AsNoTracking()
                .Include(advice => advice.Title).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(advice => advice.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .ToListAsync();
        }

        public async Task<int> AddAdvice(Advice advice)
        {
            _context.Advices.Add(advice);
            await _context.SaveChangesAsync();

            return advice.Id;
        }

        public async Task<Advice> GetAdvice(int adviceId)
        {
            return await _context.Advices
                .AsNoTracking()
                .Include(animal => animal.Title).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(animal => animal.Description).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(advice => advice.Id == adviceId);
        }

        public async Task DeleteAdvice(Advice advice)
        {
            _context.Advices.Remove(advice);
            await _context.SaveChangesAsync();
        }
    }
}
