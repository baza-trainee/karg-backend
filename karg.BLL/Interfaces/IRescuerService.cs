using karg.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IRescuerService
    {
        Task ResetPassword(string email, string newPassword);
        Task<List<AllRescuersDTO>> GetRescuers();
    }
}
