using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.DAL.Repositories
{
    public class JwtTokenRepository : IJwtTokenRepository
    {
        private readonly KargDbContext _context;

        public JwtTokenRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddJwtToken(JwtToken jwtToken)
        {
            _context.Tokens.Add(jwtToken);
            await _context.SaveChangesAsync();

            return jwtToken.Id;
        }
    }
}
