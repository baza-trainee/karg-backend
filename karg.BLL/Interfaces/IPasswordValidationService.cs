using karg.DAL.Models;

namespace karg.BLL.Interfaces
{
    public interface IPasswordValidationService
    {
        bool IsValidPassword(string password, Rescuer rescuer);
    }
}