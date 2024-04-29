using karg.BLL.DTO;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IRescuerMappingService
    {
        Task<List<AllRescuersDTO>> MapToAllRescuersDTO(List<Rescuer> rescuers);
    }
}
