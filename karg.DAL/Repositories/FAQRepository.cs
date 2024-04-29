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
    public class FAQRepository : IFAQRepository
    {
        private readonly KargDbContext _context;

        public FAQRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<FAQ>> GetFAQs()
        {
            return await _context.FAQs.AsNoTracking().ToListAsync();
        }
    }
}
