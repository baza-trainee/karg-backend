using karg.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

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
                throw new ApplicationException($"Error occurred while hashing the password: {exception.Message}");
            }
        }

        public bool VerifyHashPassword(string password, string hashedPassword)
        {
            var passwordHash = HashPassword(password);
            return string.Equals(passwordHash, hashedPassword);
        }
    }
}