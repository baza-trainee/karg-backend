using karg.BLL.DTO.Authentication;

namespace karg.BLL.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ResetPasswordResultDTO> ResetPassword(ResetPasswordDTO credentials);
        Task<AuthenticationResultDTO> Authenticate(LoginDTO loginDTO);
    }
}
