using karg.DAL.Interfaces;
using karg.BLL.DTO.Authentication;
using System.Text.RegularExpressions;
using karg.BLL.Interfaces.Email;
using karg.BLL.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;

namespace karg.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly string _baseResetPasswordUrl;

        public EmailService(
            IRescuerRepository rescuerRepository,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService,
            IConfiguration configuration)
        {
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
            _baseResetPasswordUrl = configuration["ResetPasswordUrl"];
        }

        public async Task<SendResetPasswordEmailResultDTO> SendPasswordResetEmail(string recipientEmail)
        {
            try
            {
                var rescuer = await _rescuerRepository.GetRescuerByEmail(recipientEmail);
                if (!IsValidEmail(recipientEmail) || rescuer == null)
                {
                    return new SendResetPasswordEmailResultDTO { Status = 0, Message = "Невірна електронна адреса." };
                }

                var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);
                if (string.IsNullOrEmpty(token))
                {
                    return new SendResetPasswordEmailResultDTO { Status = 0, Message = "Недійсний або прострочений токен." };
                }

                var resetPasswordLink = $"{_baseResetPasswordUrl}?token={token}";
                var emailBody = _emailTemplateService.GetPasswordResetEmailBody(recipientEmail, resetPasswordLink);

                await _emailSender.SendEmail(recipientEmail, "Відновлення паролю", emailBody);

                return new SendResetPasswordEmailResultDTO { Status = 1, Message = "Лист успішно надіслано." };
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