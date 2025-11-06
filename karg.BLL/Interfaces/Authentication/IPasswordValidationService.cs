using karg.DAL.Models;

namespace karg.BLL.Interfaces.Authentication
{
    public interface IPasswordValidationService
    {
        bool IsValidPassword(string password, Rescuer rescuer);
    }
}