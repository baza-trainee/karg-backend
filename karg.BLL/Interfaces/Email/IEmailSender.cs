namespace karg.BLL.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendEmail(string recipientEmail, string subject, string body);
    }
}