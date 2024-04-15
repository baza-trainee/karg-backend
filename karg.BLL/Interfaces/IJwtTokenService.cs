using karg.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    internal interface IJwtTokenService
    {
        string GenerateJwtToken(RescuerJwtTokenDTO rescuer);
    }
}
