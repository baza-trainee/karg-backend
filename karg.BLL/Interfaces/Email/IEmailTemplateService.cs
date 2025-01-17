namespace karg.BLL.Interfaces.Email
{
    public interface IEmailTemplateService
    {
        string GetPasswordResetEmailBody(string recipientEmail, string resetLink);
    }
}
