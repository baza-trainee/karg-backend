using AutoMapper;
using karg.BLL.DTO.Authentication;
using karg.BLL.DTO.Rescuers;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Entities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class RescuerService : IRescuerService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RescuerService(
            IJwtTokenService jwtTokenService, 
            IRescuerRepository rescuerRepository, 
            IMapper mapper, 
            IImageService imageService)
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
                    var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);

                    rescuerDto.Images = (await _imageService.GetImagesByEntity("Rescuer", rescuer.Id)).Select(image => image.Uri).ToList();
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
                if (rescuer == null) return null;

                var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);
                rescuerDto.Images = (await _imageService.GetImagesByEntity("Rescuer", rescuer.Id)).Select(image => image.Uri).ToList();

                return rescuerDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving rescuer by id: {exception.Message}");
            }
        }

        public async Task<RescuerDTO> GetRescuerByEmail(string email)
        {
            try
            {
                var rescuer = await _rescuerRepository.GetRescuerByEmail(email);
                if (rescuer == null) return null;

                var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);
                rescuerDto.Images = (await _imageService.GetImagesByEntity("Rescuer", rescuer.Id)).Select(image => image.Uri).ToList();

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
                var jwtToken = _jwtTokenService.GenerateJwtToken(new RescuerJwtTokenDTO { FullName = rescuer.FullName, Email = rescuer.Email, Role = rescuer.Role.ToString() });
                var jwtTokenId = await _jwtTokenService.AddJwtToken(jwtToken);

                rescuer.TokenId = jwtTokenId;
                rescuer.Current_Password = string.Empty;

                var rescuerId = await _rescuerRepository.Add(rescuer);
                var newImages = rescuerDto.Images.Select(uri => new CreateImageDTO
                {
                    Uri = uri,
                    RescuerId = rescuerId
                }).ToList();

                await _imageService.AddImages(newImages);
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
                var patchedRescuer = _mapper.Map<CreateAndUpdateRescuerDTO>(existingRescuer);

                patchedRescuer.Images = (await _imageService.GetImagesByEntity("Rescuer", rescuerId)).Select(image => image.Uri).ToList();

                patchDoc.ApplyTo(patchedRescuer);

                existingRescuer.FullName = patchedRescuer.FullName;
                existingRescuer.PhoneNumber = patchedRescuer.PhoneNumber;
                existingRescuer.Email = patchedRescuer.Email;

                var newJwtToken = _jwtTokenService.GenerateJwtToken(new RescuerJwtTokenDTO { FullName = existingRescuer.FullName, Email = existingRescuer.Email, Role = existingRescuer.Role.ToString() });

                await _jwtTokenService.UpdateJwtToken(existingRescuer.TokenId, newJwtToken);
                await _imageService.UpdateEntityImages("Rescuer", existingRescuer.Id, patchedRescuer.Images);
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

                await _imageService.DeleteImages("Rescuer", removedRescuer.Id);
                await _rescuerRepository.Delete(removedRescuer);
                await _jwtTokenService.DeleteJwtToken(removedRescuer.TokenId);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the rescuer: {exception.Message}");
            }
        }
    }
}