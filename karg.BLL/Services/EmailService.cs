using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using karg.BLL.DTO.Authentication;
using System.Text.RegularExpressions;

namespace karg.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailService(
            IRescuerRepository rescuerRepository,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService)
        {
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
        }

        public async Task<SendResetPasswordEmailResultDTO> SendPasswordResetEmail(string recipientEmail)
        {
            try
            {
                var rescuer = await _rescuerRepository.GetRescuerByEmail(recipientEmail);
                if (!IsValidEmail(recipientEmail) || rescuer == null)
                {
                    return new SendResetPasswordEmailResultDTO { Status = 0, Message = "Invalid email address." };
                }

                var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);
                if (string.IsNullOrEmpty(token))
                {
                    return new SendResetPasswordEmailResultDTO { Status = 0, Message = "Invalid or expired token." };
                }

                var resetLink = $"https://karg-git-dev-romanhanas-projects.vercel.app/auth/reset?token={token}";
                var emailBody = _emailTemplateService.GetPasswordResetEmailBody(recipientEmail, resetLink);

                await _emailSender.SendEmail(recipientEmail, "Відновлення паролю", emailBody);

                return new SendResetPasswordEmailResultDTO { Status = 1, Message = "Email sent successfully." };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while sending password reset email: {exception.Message}");
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}