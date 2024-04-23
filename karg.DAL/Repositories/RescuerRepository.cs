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
    public class RescuerRepository : IRescuerRepository
    {
        private readonly KargDbContext _context;

        public RescuerRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rescuer>> GetRescuers()
        {
            return await _context.Rescuers.AsNoTracking().ToListAsync();
        }
    }
}
