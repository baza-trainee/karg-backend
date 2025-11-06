using karg.DAL.Models;
using karg.DAL.Models.Enums;

namespace karg.DAL.Interfaces
{
    public interface IAnimalRepository : IBaseRepository<Animal>
    {
        Task<List<Animal>> GetAll(AnimalSortOrder sortOrder, string categoryFilter = null, string nameSearch = null);
    }
}