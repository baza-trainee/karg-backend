using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface IFAQRepository : IBaseRepository<FAQ> 
    {
        Task<List<FAQ>> GetAll(string nameSearch = null);
    }
}