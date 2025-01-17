using karg.BLL.Interfaces.Authentication;
using karg.DAL.Models;
using System.Text.RegularExpressions;

namespace karg.BLL.Services.Authentication
{
    public class PasswordValidationService : IPasswordValidationService
    {
        private readonly Regex passwordRegex = new Regex(@"^(?=.*[a-zа-я])(?=.*[A-ZА-Я])(?=.*\d)[a-zA-Zа-яА-Я\d~!?@#$%^&*()_+\[\]{}></\[""'.,:;-]{6,64}$");
        private readonly IPasswordHashService _passwordHashService;

        public PasswordValidationService(IPasswordHashService passwordHashService)
        {
            _passwordHashService = passwordHashService;
        }

        public bool IsValidPassword(string password, Rescuer rescuer)
        {
            return !string.IsNullOrEmpty(password) 
                && passwordRegex.IsMatch(password) 
                && !_passwordHashService.VerifyHashPassword(password, rescuer.Previous_Password)
                && !_passwordHashService.VerifyHashPassword(password, rescuer.Current_Password);
        }
    }
}