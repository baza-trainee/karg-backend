namespace karg.BLL.Interfaces.Authentication
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyHashPassword(string password, string hashedPassword);
    }
}