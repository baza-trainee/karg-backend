using karg.BLL.DTO.Authentication;

namespace karg.BLL.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResultDTO> Authenticate(LoginDTO loginDTO);
    }
}
