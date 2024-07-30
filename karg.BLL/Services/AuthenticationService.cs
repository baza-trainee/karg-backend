using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;

namespace karg.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(
            IPasswordHashService passwordHashService, 
            IRescuerRepository rescuerRepository, 
            IJwtTokenService jwtTokenService, 
            IPasswordValidationService passwordValidationService)
        {
            _passwordHashService = passwordHashService;
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
            _passwordValidationService = passwordValidationService;
        }

        public async Task<AuthenticationResultDTO> Authenticate(LoginDTO loginDTO)
        {
            try
            {
                var rescuer = await _rescuerRepository.GetRescuerByEmail(loginDTO.Email);
                if (rescuer == null)
                {
                    return new AuthenticationResultDTO
                    {
                        Status = 0,
                        Message = "Invalid email"
                    };
                }

                if (!_passwordHashService.VerifyHashPassword(loginDTO.Password, rescuer.Current_Password))
                {
                    return new AuthenticationResultDTO
                    {
                        Status = 0,
                        Message = "Invalid password"
                    };
                }

                var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);

                return new AuthenticationResultDTO
                {
                    Status = 1,
                    Message = "Authentication is successful",
                    Token = token,
                    RescuerId = rescuer.Id
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
                var rescuer = await _rescuerRepository.GetRescuerByEmail(credentials.Email);
                if (rescuer == null)
                {
                    return new ResetPasswordResultDTO
                    {
                        Status = 0,
                        Message = "Rescuer not found"
                    };
                }

                if (!_passwordValidationService.IsValidPassword(credentials.Password, rescuer))
                {
                    return new ResetPasswordResultDTO
                    {
                        Status = 0,
                        Message = "Invalid new password."
                    };
                }

                var newPasswordHash = _passwordHashService.HashPassword(credentials.Password);

                rescuer.Previous_Password = rescuer.Current_Password;
                rescuer.Current_Password = newPasswordHash;

                await _rescuerRepository.Update(rescuer);

                return new ResetPasswordResultDTO
                {
                    Status = 1,
                    Message = "Password reset successfully"
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while updating the password: {exception.Message}");
            }
        }
    }
}
