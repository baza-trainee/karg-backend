using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class RescuerService : IRescuerService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;

        public RescuerService(IRescuerRepository rescuerRepository, IPasswordValidationService passwordValidationService, IPasswordHashService passwordHashService)
        {
            _rescuerRepository = rescuerRepository;
            _passwordValidationService = passwordValidationService;
            _passwordHashService = passwordHashService;
        }

        public async Task ResetPassword(string email, string newPassword)
        {
            var rescuer = await _rescuerRepository.GetRescuerByEmail(email);
            if (rescuer == null)
            {
                throw new InvalidOperationException("Rescuer not found.");
            }

            var isValidPassword = _passwordValidationService.IsValidPassword(newPassword, rescuer.Current_Password);
            if (!isValidPassword)
            {
                throw new InvalidOperationException("Invalid new password.");
            }

            var newPasswordHash = _passwordHashService.HashPassword(newPassword);

            rescuer.Previous_Password = rescuer.Current_Password;
            rescuer.Current_Password = newPasswordHash;

            _rescuerRepository.UpdateRescuer(rescuer);
        }
    }
}