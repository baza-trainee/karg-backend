using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.DAL.Repositories
{
    public class JwtTokenRepository : BaseRepository<JwtToken>, IJwtTokenRepository
    {
        public JwtTokenRepository(KargDbContext context) : base(context) { }
    }
}
