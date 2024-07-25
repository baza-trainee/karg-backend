using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class FAQRepository : BaseRepository<FAQ>, IFAQRepository
    {
        public FAQRepository(KargDbContext context) : base(context) { }

        public override async Task<List<FAQ>> GetAll()
        {
            return await _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .ToListAsync();
        }
        
        public override async Task<FAQ> GetById(int faqId)
        {
            return await _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(faq => faq.Id == faqId);
        }
    }
}
