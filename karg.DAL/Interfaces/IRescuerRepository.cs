using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IRescuerRepository
    {
        Task<Rescuer> GetRescuerByEmail(string email);
        Task<List<Rescuer>> GetRescuers();
        Task<int> AddRescuer(Rescuer rescuer);
        Task<Rescuer> UpdateRescuer(Rescuer updatedRescuer);
    }
}