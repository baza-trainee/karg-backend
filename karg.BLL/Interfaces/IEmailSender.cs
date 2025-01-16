namespace karg.BLL.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string recipientEmail, string subject, string body);
    }
}