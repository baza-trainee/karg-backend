using AutoMapper;
using karg.BLL.DTO.Authentication;
using karg.BLL.DTO.Rescuers;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Rescuers;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Services.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;

namespace karg.BLL.Services.Rescuers
{
    public class RescuerService : IRescuerService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RescuerService(IJwtTokenService jwtTokenService, IRescuerRepository rescuerRepository, IPasswordValidationService passwordValidationService, IPasswordHashService passwordHashService, IMapper mapper, IImageService imageService)
        {
            _jwtTokenService = jwtTokenService;
            _rescuerRepository = rescuerRepository;
            _passwordValidationService = passwordValidationService;
            _passwordHashService = passwordHashService;
            _imageService = imageService;
            _mapper = mapper;
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

            await _rescuerRepository.UpdateRescuer(rescuer);
        }

        public async Task<List<AllRescuersDTO>> GetRescuers()
        {
            try
            {
                var rescuers = await _rescuerRepository.GetRescuers();
                var rescuersDto = new List<AllRescuersDTO>();

                foreach (var rescuer in rescuers)
                {
                    var rescuerImage = await _imageService.GetImageById(rescuer.ImageId);
                    var rescuerDto = _mapper.Map<AllRescuersDTO>(rescuer);

                    rescuerDto.Image = rescuerImage;
                    rescuersDto.Add(rescuerDto);
                }

                return rescuersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of rescuers: {exception.Message}");
            }
        }

        public async Task CreateRescuer(CreateAndUpdateRescuerDTO rescuerDto)
        {
            try
            {
                var rescuer = _mapper.Map<Rescuer>(rescuerDto);
                var newImage = new CreateImageDTO
                {
                    Uri = rescuerDto.Image,
                    AnimalId = null,
                };
                var imageId = await _imageService.AddImage(newImage);
                var jwtToken = _jwtTokenService.GenerateJwtToken(new RescuerJwtTokenDTO { FullName = rescuer.FullName, Email = rescuer.Email, Role = rescuer.Role.ToString() });
                var jwtTokenId = await _jwtTokenService.AddJwtToken(jwtToken);

                rescuer.TokenId = jwtTokenId;   
                rescuer.ImageId = imageId;
                rescuer.Current_Password = string.Empty;

                await _rescuerRepository.AddRescuer(rescuer);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the rescuer: {exception.Message}");
            }
        }

        public async Task DeleteRescuer(int rescuerId)
        {
            try
            {
                var removedRescuer = await _rescuerRepository.GetRescuer(rescuerId);

                await _rescuerRepository.DeleteRescuer(removedRescuer);
                await _imageService.DeleteImage(removedRescuer.ImageId);
                await _jwtTokenService.DeleteJwtToken(removedRescuer.TokenId);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the rescuer: {exception.Message}");
            }
        }
    }
}