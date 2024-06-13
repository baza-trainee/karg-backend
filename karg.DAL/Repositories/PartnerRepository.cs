using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Partner> GetPartner(int partnerId)
        {
            return await _context.Partners.AsNoTracking().FirstOrDefaultAsync(partner => partner.Id == partnerId);
        }

        public async Task<int> AddPartner(Partner partner)
        {
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();

            return partner.Id;
        }

        public async Task DeletePartner(Partner partner)
        {
            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
        }
    }
}
