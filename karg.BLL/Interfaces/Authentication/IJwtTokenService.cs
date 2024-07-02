using karg.BLL.DTO.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(RescuerJwtTokenDTO rescuer);
        Task<int> AddJwtToken(string token);
    }
}
