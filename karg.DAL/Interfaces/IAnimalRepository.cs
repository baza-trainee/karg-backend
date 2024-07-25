using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface IAnimalRepository : IBaseRepository<Animal>
    {
        Task<List<Animal>> GetAll(string categoryFilter = null, string nameSearch = null);
    }
}
