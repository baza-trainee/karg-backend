using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAnimals(int page, int pageSize, string categoryFilter = null, string nameSearch = null);
    }
}
