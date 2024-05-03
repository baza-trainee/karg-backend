using AutoMapper;
using karg.BLL.DTO.Rescuers;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Rescuers;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Rescuers
{
    public class RescuerService : IRescuerService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IPasswordValidationService _passwordValidationService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RescuerService(IRescuerRepository rescuerRepository, IPasswordValidationService passwordValidationService, IPasswordHashService passwordHashService, IMapper mapper, IImageService imageService)
        {
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

                    rescuerDto.Image = rescuerImage.Uri;
                    rescuersDto.Add(rescuerDto);
                }

                return rescuersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of rescuers.", exception);
            }
        }
    }
}