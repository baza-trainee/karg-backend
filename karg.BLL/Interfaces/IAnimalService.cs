using karg.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IAnimalService
    {
        Task<List<AllAnimalsDTO>> GetAnimals(AnimalsFilterDTO filter);
    }
}
