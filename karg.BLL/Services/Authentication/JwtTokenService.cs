using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Authentication;
using karg.BLL.Interfaces.Authentication;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace karg.BLL.Services.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenRepository _jwtTokenRepository;

        public JwtTokenService(IConfiguration configuration, IJwtTokenRepository jwtTokenRepository)
        {
            _configuration = configuration;
            _jwtTokenRepository = jwtTokenRepository;
        }

        public async Task<string> GetJwtTokenById(int tokenId)
        {
            try
            {
                var jwtToken = await _jwtTokenRepository.GetById(tokenId);

                if (jwtToken == null)
                {
                    return null;
                }

                return jwtToken.Token;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving JWT token by id: {exception.Message}");
            }
        }

        public async Task<int> AddJwtToken(string token)
        {
            try
            {
                var jwtToken = new JwtToken
                {
                    Token = token
                };
                
                return await _jwtTokenRepository.Add(jwtToken);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the JWT token: {exception.Message}");
            }
        }

        public string GenerateJwtToken(RescuerJwtTokenDTO rescuer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Fullname", rescuer.FullName),
                new Claim("Role", rescuer.Role),
                new Claim("Email", rescuer.Email),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(365000),
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }

        public async Task DeleteJwtToken(int tokenId)
        {
            try
            {
                var removedToken = await _jwtTokenRepository.GetById(tokenId);

                await _jwtTokenRepository.Delete(removedToken);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the JWT token: {exception.Message}");
            }
        }
    }
}
