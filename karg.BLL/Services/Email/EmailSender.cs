using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using karg.BLL.Interfaces.Email;

namespace karg.BLL.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailSender(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            _smtpServer = emailSettings["SmtpServer"] ?? throw new ArgumentNullException("SMTP Server is not configured.");
            _smtpPort = int.TryParse(emailSettings["SmtpPort"], out var port) ? port : throw new ArgumentException("Invalid SMTP port.");
            _smtpUsername = emailSettings["SmtpUsername"] ?? throw new ArgumentNullException("SMTP username is not configured.");
            _smtpPassword = emailSettings["SmtpPassword"] ?? throw new ArgumentNullException("SMTP password is not configured.");
        }

        public async Task SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUsername, "Команда КАРГ"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while sending email: {exception.Message}");
            }
        }
    }
}