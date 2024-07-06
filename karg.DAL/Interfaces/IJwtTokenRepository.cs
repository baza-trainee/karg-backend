using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IJwtTokenRepository
    {
        Task<JwtToken> GetJwtToken(int tokenId);
        Task<int> AddJwtToken(JwtToken jwtToken);
    }
}
