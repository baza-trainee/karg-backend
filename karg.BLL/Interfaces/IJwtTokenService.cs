using karg.BLL.DTO.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GetJwtTokenById(int tokenId);
        ClaimsPrincipal DecodeJwtToken(string token);
        string GenerateJwtToken(RescuerJwtTokenDTO rescuer);
        Task<int> AddJwtToken(string token);
        Task UpdateJwtToken(int tokenId, string updatedToken);
        Task DeleteJwtToken(int tokenId);
    }
}
