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
        Task<Rescuer> UpdateRescuer(Rescuer updatedRescuer);
        Task<Rescuer> GetRescuerByEmail(string email);
    }
}