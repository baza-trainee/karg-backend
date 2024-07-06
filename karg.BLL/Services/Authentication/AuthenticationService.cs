using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace karg.BLL.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(IPasswordHashService passwordHashService, IRescuerRepository rescuerRepository, IJwtTokenService jwtTokenService)
        {
            _passwordHashService = passwordHashService;
            _rescuerRepository = rescuerRepository;
            _jwtTokenService = jwtTokenService;
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
                throw new ApplicationException("Error during authentication.", exception);
            }
        }
    }
}
