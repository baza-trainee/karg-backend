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
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Rescuers
{
    public class RescuerService : IRescuerService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RescuerService(IJwtTokenService jwtTokenService, IRescuerRepository rescuerRepository, IMapper mapper, IImageService imageService)
        {
            _jwtTokenService = jwtTokenService;
            _rescuerRepository = rescuerRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<List<RescuerDTO>> GetRescuers()
        {
            try
            {
                var rescuers = await _rescuerRepository.GetAll();
                var rescuersDto = new List<RescuerDTO>();

                foreach (var rescuer in rescuers)
                {
                    var rescuerImage = await _imageService.GetImageById(rescuer.ImageId);
                    var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);

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

        public async Task<RescuerDTO> GetRescuerById(int rescuerId)
        {
            try
            {
                var rescuer = await _rescuerRepository.GetById(rescuerId);

                if (rescuer == null)
                {
                    return null;
                }

                var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);
                var rescuerImage = await _imageService.GetImageById(rescuer.ImageId);

                rescuerDto.Image = rescuerImage;

                return rescuerDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving rescuer by id: {exception.Message}");
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

                await _rescuerRepository.Add(rescuer);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the rescuer: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateRescuerDTO> UpdateRescuer(int rescuerId, JsonPatchDocument<CreateAndUpdateRescuerDTO> patchDoc)
        {
            try
            {
                var existingRescuer = await _rescuerRepository.GetById(rescuerId);
                var patchedRescuer= _mapper.Map<CreateAndUpdateRescuerDTO>(existingRescuer);

                var rescuerImage = await _imageService.GetImageById(existingRescuer.ImageId);
                patchedRescuer.Image = rescuerImage;

                patchDoc.ApplyTo(patchedRescuer);

                existingRescuer.FullName = patchedRescuer.FullName;
                existingRescuer.PhoneNumber = patchedRescuer.PhoneNumber;
                existingRescuer.Email = patchedRescuer.Email;

                await _imageService.UpdateImage(existingRescuer.ImageId, patchedRescuer.Image);
                await _rescuerRepository.Update(existingRescuer);

                return patchedRescuer;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the rescuer: {exception.Message}");
            }
        }

        public async Task DeleteRescuer(int rescuerId)
        {
            try
            {
                var removedRescuer = await _rescuerRepository.GetById(rescuerId);

                await _rescuerRepository.Delete(removedRescuer);
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