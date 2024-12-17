using karg.BLL.DTO.Authentication;

namespace karg.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<SendResetPasswordEmailResultDTO> SendPasswordResetEmail(string recipientEmail);
        bool IsValidEmail(string email);
    }
}