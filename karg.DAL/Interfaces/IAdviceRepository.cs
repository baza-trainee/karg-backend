using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface IAdviceRepository : IBaseRepository<Advice> 
    {
        Task<List<Advice>> GetAll(string nameSearch = null);
    }
}