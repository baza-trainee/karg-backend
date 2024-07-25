using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class LocalizationRepository : BaseRepository<Localization>, ILocalizationRepository
    {
        public LocalizationRepository(KargDbContext context) : base(context) { }

        public async Task<List<Localization>> GetLocalizationBySetId(int localizationSetId)
        {
            return await _context.Localizations.AsNoTracking().Where(localization => localization.LocalizationSetId == localizationSetId).ToListAsync();
        }

        public override async Task Update(Localization updatedLocalisation)
        {

            var existingLocalization = await _context.Localizations.AsNoTracking().FirstOrDefaultAsync(localization =>
                    localization.LocalizationSetId == updatedLocalisation.LocalizationSetId &&
                    localization.CultureCode == updatedLocalisation.CultureCode);

            existingLocalization.Value = updatedLocalisation.Value;

            _context.Localizations.Update(existingLocalization);
            await _context.SaveChangesAsync();
        }
    }
}
