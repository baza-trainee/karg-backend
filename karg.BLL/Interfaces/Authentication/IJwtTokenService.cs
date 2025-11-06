using karg.BLL.DTO.Authentication;
using System.Security.Claims;

namespace karg.BLL.Interfaces.Authentication
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
