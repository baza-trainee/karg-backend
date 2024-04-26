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
    public class PartnerRepository : IPartnerRepository
    {
        private readonly KargDbContext _context;

        public PartnerRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Partner>> GetPartners()
        {
            return await _context.Partners.AsNoTracking().ToListAsync();
        }
    }
}
