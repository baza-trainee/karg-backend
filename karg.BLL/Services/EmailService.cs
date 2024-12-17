using karg.BLL.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using karg.DAL.Interfaces;
using karg.BLL.DTO.Authentication;
using System.Text.RegularExpressions;

namespace karg.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailService(IConfiguration configuration, IRescuerRepository rescuerRepository, IJwtTokenService jwtTokenService)
        {
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
            var emailSettings = configuration.GetSection("EmailSettings");
            smtpServer = emailSettings["SmtpServer"];
            smtpPort = int.Parse(emailSettings["SmtpPort"]);
            smtpUsername = emailSettings["SmtpUsername"];
            smtpPassword = emailSettings["SmtpPassword"];
        }

        public async Task<SendResetPasswordEmailResultDTO> SendPasswordResetEmail(string recipientEmail)
        {
            try
            {
                var (success, message, resetLink) = await GetPasswordResetLinkAsync(recipientEmail);
                if (!success)
                {
                    return new SendResetPasswordEmailResultDTO
                    {
                        Status = 0,
                        Message = message
                    };
                }
                string emailSubject = "Відновлення паролю";
                string htmlContent = $@"
                <div>
                  <table style='margin: 0 auto; width: 600px; text-align: center; box-sizing: border-box; font-family: Arial, sans-serif; font-size: 16px; color: #070707; line-height: 22px; border-spacing: 0; border-collapse: collapse;'>
                    <tr>
                      <td style='padding: 24px 40px;'>Вітаємо!</td>
                    </tr>
                    <tr>
                      <td style='padding: 0 40px 21px 40px'>
                        Ми отримали запит на відновлення паролю для облікового запису
                        контрольної панелі на сайті КАРГ, що пов'язаний з Вашою електронною
                        адресою {recipientEmail}.
                      </td>
                    </tr>
                    <tr>
                      <td style='padding: 0 40px 16px 40px;'>
                        Якщо Ви дійсно бажаєте відновити пароль, можете це зробити натиснувши
                        на посилання: <a href='{resetLink}' target='_blank'>{resetLink.Substring(0, 135)}...</a>
                      </td>
                    </tr>
                    <tr>
                      <td style='padding: 0 40px 21px 40px'>
                        Якщо Ви не подавали запит до відновлення паролю - не відповідайте на
                        цей лист.
                      </td>
                    </tr>
                    <tr>
                      <td style='padding: 0 40px 32px 40px;'>З повагою, команда КАРГ.</td>
                    </tr>
                  </table>
                </div>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername, "Команда КАРГ"),
                    Subject = emailSubject,
                    Body = htmlContent,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(recipientEmail));

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return new SendResetPasswordEmailResultDTO
                {
                    Status = 1,
                    Message = "Email sent successfully"
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while sending password reset email: {exception.Message}");
            }
        }

        private async Task<(bool Success, string Message, string ResetLink)> GetPasswordResetLinkAsync(string recipientEmail)
        {
            var rescuer = await _rescuerRepository.GetRescuerByEmail(recipientEmail);
            if (!IsValidEmail(recipientEmail) || rescuer == null)
            {
                return (false, "Invalid email address.", null);
            }

            var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);
            if (string.IsNullOrEmpty(token))
            {
                return (false, "Invalid or expired token.", null);
            }

            var resetLink = $"https://karg-git-dev-romanhanas-projects.vercel.app/auth/reset?token={token}";
            return (true, string.Empty, resetLink);
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
