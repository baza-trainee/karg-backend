using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class FAQRepository : BaseRepository<FAQ>, IFAQRepository
    {
        public FAQRepository(KargDbContext context) : base(context) { }

        public async Task<List<FAQ>> GetAll(string nameSearch = null)
        {
            var faqs = _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .AsSplitQuery();

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                string lowerNameSearch = nameSearch.ToLower();
                faqs = faqs.Where(faq =>
                    faq.Question.Localizations.Any(localization =>
                        localization.Value.ToLower().Contains(lowerNameSearch)));
            }

            return await faqs.ToListAsync();
        }

        public override async Task<FAQ> GetById(int faqId)
        {
            return await _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .AsSplitQuery()
                .FirstOrDefaultAsync(faq => faq.Id == faqId);
        }
    }
}
