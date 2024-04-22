using karg.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashedBytes);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error occurred while hashing the password.", exception);
            }
        }

        public bool VerifyHashPassword(string password, string hashedPassword)
        {
            try
            {
                return string.Equals(HashPassword(password), hashedPassword);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error occurred while verifying the password hash.", exception);
            }
        }
    }
}