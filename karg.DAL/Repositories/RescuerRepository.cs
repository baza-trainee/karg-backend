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

        public async Task<Rescuer> UpdateRescuer(Rescuer updatedRescuer)
        {
            var existingRescuer = await _context.Rescuers.FindAsync(updatedRescuer.Id);

            if (existingRescuer == null)
            {
                return null;
            }

            existingRescuer.FullName = updatedRescuer.FullName;
            existingRescuer.Email = updatedRescuer.Email;
            existingRescuer.PhoneNumber = updatedRescuer.PhoneNumber;
            existingRescuer.Current_Password = updatedRescuer.Current_Password;
            existingRescuer.Previous_Password = updatedRescuer.Previous_Password;
            existingRescuer.Role = updatedRescuer.Role;
            existingRescuer.ImageId = updatedRescuer.ImageId;

            await _context.SaveChangesAsync();
            return existingRescuer;
        }

        public async Task<List<Rescuer>> GetRescuers()
        {
            return await _context.Rescuers.AsNoTracking().ToListAsync();
        }
    }
}