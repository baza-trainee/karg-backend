using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public class RescuerRepository : BaseRepository<Rescuer>, IRescuerRepository
    {
        public RescuerRepository(KargDbContext context) : base(context) { }

        public async Task<Rescuer> GetRescuerByEmail(string email)
        {
            return await _context.Rescuers.FirstOrDefaultAsync(rescuer => rescuer.Email == email);
        }
    }
}