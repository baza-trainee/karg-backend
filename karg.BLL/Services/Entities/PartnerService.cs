using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Partners;
using karg.BLL.Interfaces.Entities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public PartnerService(IPartnerRepository partnerRepository, IMapper mapper, IImageService imageService)
        {
            _partnerRepository = partnerRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<List<PartnerDTO>> GetPartners()
        {
            try
            {
                var partners = await _partnerRepository.GetAll();
                var partnersDto = new List<PartnerDTO>();

                foreach (var partner in partners)
                {
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);

                    partnerDto.Images = (await _imageService.GetImagesByEntity("Partner", partner.Id)).Select(image => image.Uri).ToList();
                    partnersDto.Add(partnerDto);
                }

                return partnersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of partners: {exception.Message}");
            }
        }

        public async Task<PartnerDTO> GetPartnerById(int partnerId)
        {
            try
            {
                var partner = await _partnerRepository.GetById(partnerId);
                if (partner == null) return null;

                var partnerDto = _mapper.Map<PartnerDTO>(partner);
                partnerDto.Images = (await _imageService.GetImagesByEntity("Partner", partner.Id)).Select(image => image.Uri).ToList();

                return partnerDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving partner by id: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdatePartnerDTO> CreatePartner(CreateAndUpdatePartnerDTO partnerDto)
        {
            try
            {
                var partner = _mapper.Map<Partner>(partnerDto);
                var partnerId = await _partnerRepository.Add(partner);

                if (partnerDto.Images != null && partnerDto.Images.Any())
                {
                    var imageBytesList = partnerDto.Images
                        .Select(Convert.FromBase64String)
                        .ToList();

                    await _imageService.AddImages(nameof(Partner), partnerId, imageBytesList);
                }

                partnerDto.Images = (await _imageService.GetImagesByEntity(nameof(Partner), partnerId)).Select(image => image.Uri).ToList();

                return partnerDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the partner: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdatePartnerDTO> UpdatePartner(int partnerId, JsonPatchDocument<CreateAndUpdatePartnerDTO> patchDoc)
        {
            try
            {
                var existingPartner = await _partnerRepository.GetById(partnerId);
                var patchedPartner = _mapper.Map<CreateAndUpdatePartnerDTO>(existingPartner);

                patchDoc.ApplyTo(patchedPartner);

                if (patchDoc.Operations.Any(op => op.path == "/images"))
                {
                    var imageBytesList = patchedPartner.Images
                                .Select(Convert.FromBase64String)
                                .ToList();
                    await _imageService.UpdateEntityImages(nameof(Partner), partnerId, imageBytesList);
                }

                existingPartner.Name = patchedPartner.Name;
                existingPartner.Uri = patchedPartner.Uri;

                await _partnerRepository.Update(existingPartner);
                patchedPartner.Images = (await _imageService.GetImagesByEntity(nameof(Partner), partnerId)).Select(image => image.Uri).ToList();

                return patchedPartner;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the partner: {exception.Message}");
            }
        }

        public async Task DeletePartner(int partnerId)
        {
            try
            {
                var removedPartner = await _partnerRepository.GetById(partnerId);

                await _imageService.DeleteImages("Partner", removedPartner.Id);
                await _partnerRepository.Delete(removedPartner);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the partner: {exception.Message}");
            }
        }
    }
}
