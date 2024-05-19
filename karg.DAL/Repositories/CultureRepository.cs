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
    public class CultureRepository : ICultureRepository
    {
        private readonly KargDbContext _context;

        public CultureRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Culture>> GetCultures()
        {
            return await _context.Cultures.AsNoTracking().ToListAsync();
        }
    }
}
