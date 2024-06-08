﻿using karg.DAL.Context;
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
    public class FAQRepository : IFAQRepository
    {
        private readonly KargDbContext _context;

        public FAQRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<FAQ>> GetFAQs()
        {
            return await _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .ToListAsync();
        }

        public async Task<int> AddFAQ(FAQ faq)
        {
            _context.FAQs.Add(faq);
            await _context.SaveChangesAsync();

            return faq.Id;
        }
        
        public async Task<FAQ> GetFAQ(int faqId)
        {
            return await _context.FAQs
                .AsNoTracking()
                .Include(faq => faq.Question).ThenInclude(localizationSet => localizationSet.Localizations)
                .Include(faq => faq.Answer).ThenInclude(localizationSet => localizationSet.Localizations)
                .FirstOrDefaultAsync(faq => faq.Id == faqId);
        }

        public async Task UpdateFAQ(FAQ updatedFAQ)
        {
            var existingFAQ = await _context.FAQs.FindAsync(updatedFAQ.Id);

            _context.Entry(existingFAQ).CurrentValues.SetValues(updatedFAQ);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFAQ(FAQ faq)
        {
            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
        }
    }
}
