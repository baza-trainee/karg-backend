using karg.BLL.DTO.Authentication;

namespace karg.BLL.Interfaces.Email
{
    public interface IEmailService
    {
        Task<SendResetPasswordEmailResultDTO> SendPasswordResetEmail(string recipientEmail);
        bool IsValidEmail(string email);
    }
}