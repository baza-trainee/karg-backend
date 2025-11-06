using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.DAL.Repositories
{
    public class CultureRepository : BaseRepository<Culture>, ICultureRepository
    {
        public CultureRepository(KargDbContext context) : base(context) { }
    }
}
