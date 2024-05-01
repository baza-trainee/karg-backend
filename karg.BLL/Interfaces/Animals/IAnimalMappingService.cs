using karg.BLL.DTO.Animals;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Animals
{
    public interface IAnimalMappingService
    {
        Task<List<AllAnimalsDTO>> MapToAllAnimalsDTO(List<Animal> animals);
    }
}
