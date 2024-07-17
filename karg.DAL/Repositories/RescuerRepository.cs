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

        public async Task<Rescuer> GetRescuerByEmail(string email)
        {
            return await _context.Rescuers.FirstOrDefaultAsync(rescuer => rescuer.Email == email);
        }

        public async Task<int> AddRescuer(Rescuer rescuer)
        {
            _context.Rescuers.Add(rescuer);
            await _context.SaveChangesAsync();

            return rescuer.Id;
        }

        public async Task UpdateRescuer(Rescuer updatedRescuer)
        {
            var existingRescuer = await _context.Rescuers.FindAsync(updatedRescuer.Id);

            _context.Entry(existingRescuer).CurrentValues.SetValues(updatedRescuer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rescuer>> GetRescuers()
        {
            return await _context.Rescuers.AsNoTracking().ToListAsync();
        }

        public async Task<Rescuer> GetRescuer(int rescuerId)
        {
            return await _context.Rescuers.AsNoTracking().FirstOrDefaultAsync(rescuer => rescuer.Id == rescuerId);
        }

        public async Task DeleteRescuer(Rescuer rescuer)
        {
            _context.Rescuers.Remove(rescuer);
            await _context.SaveChangesAsync();
        }
    }
}