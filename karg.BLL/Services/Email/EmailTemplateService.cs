using karg.BLL.Interfaces.Email;

namespace karg.BLL.Services.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetPasswordResetEmailBody(string recipientEmail, string resetLink)
        {
            var shortenedLink = resetLink.Length > 135 ? resetLink.Substring(0, 135) + "..." : resetLink;

            return $@"
            <div>
              <table style='margin: 0 auto; width: 600px; text-align: center; box-sizing: border-box; 
                            font-family: Arial, sans-serif; font-size: 16px; color: #070707; line-height: 22px; 
                            border-spacing: 0; border-collapse: collapse;'>
                <tr>
                  <td style='padding: 24px 40px;'>Вітаємо!</td>
                </tr>
                <tr>
                  <td style='padding: 0 40px 21px 40px'>
                    Ми отримали запит на відновлення паролю для облікового запису
                    контрольної панелі на сайті КАРГ, що пов'язаний з Вашою електронною адресою {recipientEmail}.
                  </td>
                </tr>
                <tr>
                  <td style='padding: 0 40px 16px 40px;'>
                    Якщо Ви дійсно бажаєте відновити пароль, можете це зробити, натиснувши
                    на посилання: <a href='{resetLink}' target='_blank'>{shortenedLink}</a>
                  </td>
                </tr>
                <tr>
                  <td style='padding: 0 40px 21px 40px'>
                    Якщо Ви не подавали запит до відновлення паролю - не відповідайте на цей лист.
                  </td>
                </tr>
                <tr>
                  <td style='padding: 0 40px 32px 40px;'>З повагою, команда КАРГ.</td>
                </tr>
              </table>
            </div>";
        }
    }
}