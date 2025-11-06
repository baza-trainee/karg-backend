using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Email;
using karg.DAL.Interfaces;
using karg.DAL.Models.Enums;

namespace karg.BLL.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;

        public AuthenticationService(
            IPasswordHashService passwordHashService, 
            IRescuerRepository rescuerRepository, 
            IJwtTokenService jwtTokenService, 
            IPasswordValidationService passwordValidationService,
            IEmailService emailService)
        {
            _passwordHashService = passwordHashService;
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
            _passwordValidationService = passwordValidationService;
            _emailService = emailService;
        }

        public async Task<AuthenticationResultDTO> Authenticate(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null ||
                        !_emailService.IsValidEmail(loginDTO.Email) ||
                        string.IsNullOrWhiteSpace(loginDTO.Password))
                {
                    return new AuthenticationResultDTO
                    {
                        Status = 0,
                        Message = "Невірний формат електронної пошти або пароля."
                    };
                }

                var rescuer = await _rescuerRepository.GetRescuerByEmail(loginDTO.Email);
                if (rescuer == null ||
                    !_passwordHashService.VerifyHashPassword(loginDTO.Password, rescuer.Current_Password))
                {
                    return new AuthenticationResultDTO
                    {
                        Status = 0,
                        Message = "Невірна електронна пошта або пароль."
                    };
                }

                var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);

                return new AuthenticationResultDTO
                {
                    Status = 1,
                    Message = "Автентифікація пройшла успішно.",
                    Token = token,
                    RescuerId = rescuer.Id,
                    IsDirector = rescuer.Role == RescuerRole.Director
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error during authentication: {exception.Message}");
            }
        }

        public async Task<ResetPasswordResultDTO> ResetPassword(ResetPasswordDTO credentials)
        {
            try
            {
                if (credentials == null ||
                    string.IsNullOrWhiteSpace(credentials.Token) ||
                    string.IsNullOrWhiteSpace(credentials.Password))
                {
                    return new ResetPasswordResultDTO
                    {
                        Status = 0,
                        Message = "Необхідно вказати токен і новий пароль."
                    };
                }

                var email = _jwtTokenService.DecodeJwtToken(credentials.Token).Claims.FirstOrDefault(claim => claim.Type == "Email").Value;
                var rescuer = await _rescuerRepository.GetRescuerByEmail(email);
                if (rescuer == null)
                {
                    return new ResetPasswordResultDTO
                    {
                        Status = 0,
                        Message = "Працівника не знайдено."
                    };
                }

                if (!_passwordValidationService.IsValidPassword(credentials.Password, rescuer))
                {
                    return new ResetPasswordResultDTO
                    {
                        Status = 0,
                        Message = "Ви ввели невідповідний пароль."
                    };
                }

                var newPasswordHash = _passwordHashService.HashPassword(credentials.Password);

                rescuer.Previous_Password = rescuer.Current_Password;
                rescuer.Current_Password = newPasswordHash;

                await _rescuerRepository.Update(rescuer);

                return new ResetPasswordResultDTO
                {
                    Status = 1,
                    Message = "Пароль успішно змінено."
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while updating the password: {exception.Message}");
            }
        }
    }
}