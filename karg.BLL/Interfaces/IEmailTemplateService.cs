namespace karg.BLL.Interfaces
{
    public interface IEmailTemplateService
    {
        string GetPasswordResetEmailBody(string recipientEmail, string resetLink);
    }
}
