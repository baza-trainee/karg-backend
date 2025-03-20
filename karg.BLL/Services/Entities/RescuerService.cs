using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Authentication;
using karg.BLL.DTO.Rescuers;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class RescuerService : IRescuerService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IPaginationService<Rescuer> _paginationService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RescuerService(
            IJwtTokenService jwtTokenService, 
            IRescuerRepository rescuerRepository,
            IPaginationService<Rescuer> paginationService,
            IMapper mapper, 
            IImageService imageService)
        {
            _jwtTokenService = jwtTokenService;
            _rescuerRepository = rescuerRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RescuerDTO>> GetRescuers(RescuersFilterDTO filter)
        {
            try
            {
                var rescuers = await _rescuerRepository.GetAll();
                var paginatedRescuers = await _paginationService.PaginateWithTotalPages(rescuers, filter.Page, filter.PageSize);
                var rescuersDto = new List<RescuerDTO>();

                foreach (var rescuer in paginatedRescuers.Items)
                {
                    var rescuerDto = _mapper.Map<RescuerDTO>(rescuer);

                    rescuerDto.Images = (await _imageService.GetImagesByEntity("Rescuer", rescuer.Id)).Select(image => image.Uri).ToList();
                    rescuersDto.Add(rescuerDto);
                }

                return new PaginatedResult<RescuerDTO>
                {
                    Items = rescuersDto,
                    TotalPages = paginatedRescuers.TotalPages,
                    TotalItems = rescuers.Count()
                };
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

        public async Task<CreateAndUpdateRescuerDTO> CreateRescuer(CreateAndUpdateRescuerDTO rescuerDto)
        {
            try
            {
                var rescuer = _mapper.Map<Rescuer>(rescuerDto);
                var jwtTokenId = await _jwtTokenService.AddJwtToken(string.Empty);

                rescuer.TokenId = jwtTokenId;
                rescuer.Current_Password = string.Empty;

                var rescuerId = await _rescuerRepository.Add(rescuer);
                var jwtToken = _jwtTokenService.GenerateJwtToken(new RescuerJwtTokenDTO
                {
                    Id = rescuerId,
                    FullName = rescuer.FullName,
                    Email = rescuer.Email,
                    Role = rescuer.Role.ToString()
                });

                await _jwtTokenService.UpdateJwtToken(jwtTokenId, jwtToken);

                rescuerDto.Images = await _imageService.UploadImages(nameof(Rescuer), rescuerId, rescuerDto.Images, false);

                return rescuerDto;
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

                patchDoc.ApplyTo(patchedRescuer);

                if (patchDoc.Operations.Any(op => op.path == "/images"))
                {
                    patchedRescuer.Images = await _imageService.UploadImages(nameof(Rescuer), rescuerId, patchedRescuer.Images, true);
                }

                existingRescuer.FullName = patchedRescuer.FullName;
                existingRescuer.PhoneNumber = patchedRescuer.PhoneNumber;
                existingRescuer.Email = patchedRescuer.Email;

                var newJwtToken = _jwtTokenService.GenerateJwtToken(new RescuerJwtTokenDTO { FullName = existingRescuer.FullName, Email = existingRescuer.Email, Role = existingRescuer.Role.ToString() });

                await _jwtTokenService.UpdateJwtToken(existingRescuer.TokenId, newJwtToken);
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