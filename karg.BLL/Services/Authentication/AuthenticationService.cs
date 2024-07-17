using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace karg.BLL.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(IPasswordHashService passwordHashService, IRescuerRepository rescuerRepository, IJwtTokenService jwtTokenService, IPasswordValidationService passwordValidationService)
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
                var authenticationResult = new AuthenticationResultDTO
                {
                    Status = 0,
                    Token = null
                };

                if (rescuer == null)
                {
                    authenticationResult.Message = "Invalid email";
                    return authenticationResult;
                }

                if (!_passwordHashService.VerifyHashPassword(loginDTO.Password, rescuer.Current_Password))
                {
                    authenticationResult.Message = "Invalid password";
                    return authenticationResult;
                }

                var token = await _jwtTokenService.GetJwtTokenById(rescuer.TokenId);

                authenticationResult.Status = 1;
                authenticationResult.Message = "Authentication is successful";
                authenticationResult.Token = token;

                return authenticationResult;
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
                var resetPasswordResult = new ResetPasswordResultDTO
                {
                    Status = 0,
                };
                var rescuer = await _rescuerRepository.GetRescuerByEmail(credentials.Email);
                if (rescuer == null)
                {
                    resetPasswordResult.Message = "Rescuer not found.";
                    return resetPasswordResult;
                }

                var isValidPassword = _passwordValidationService.IsValidPassword(credentials.Password, rescuer.Current_Password);
                if (!isValidPassword)
                {
                    resetPasswordResult.Message = "Invalid new password.";
                    return resetPasswordResult;
                }

                var newPasswordHash = _passwordHashService.HashPassword(credentials.Password);

                rescuer.Previous_Password = rescuer.Current_Password;
                rescuer.Current_Password = newPasswordHash;

                await _rescuerRepository.UpdateRescuer(rescuer);

                resetPasswordResult.Status = 1;
                resetPasswordResult.Message = "Password reset successfully.";

                return resetPasswordResult;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error occurred while updating the password: {exception.Message}");
            }
        }
    }
}
