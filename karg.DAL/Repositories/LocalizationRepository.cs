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
    public class LocalizationRepository : ILocalizationRepository
    {
        private readonly KargDbContext _context;

        public LocalizationRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Localization>> GetLocalization(int localizationSetId)
        {
            return await _context.Localizations.AsNoTracking().Where(localization => localization.LocalizationSetId == localizationSetId).ToListAsync();
        }

        public async Task UpdateLocalization(Localization updatedLocalisation)
        {

            var existingLocalization = await _context.Localizations.AsNoTracking().FirstOrDefaultAsync(localization =>
                    localization.LocalizationSetId == updatedLocalisation.LocalizationSetId &&
                    localization.CultureCode == updatedLocalisation.CultureCode);

            existingLocalization.Value = updatedLocalisation.Value;

            _context.Localizations.Update(existingLocalization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocalization(Localization localization)
        {
            _context.Localizations.Remove(localization);
            await _context.SaveChangesAsync();
        }
    }
}
