﻿using AutoMapper;
using karg.BLL.DTO.Partners;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Partners;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Services.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Partners
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
                var partners = await _partnerRepository.GetPartners();
                var partnersDto = new List<PartnerDTO>();

                foreach (var partner in partners)
                {
                    var partnerImage = await _imageService.GetImageById(partner.ImageId);
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);

                    partnerDto.Image = partnerImage;
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
                var partner = await _partnerRepository.GetPartner(partnerId);
                
                if(partner == null)
                {
                    return null;
                }

                var partnerDto = _mapper.Map<PartnerDTO>(partner);
                var partnerImage = await _imageService.GetImageById(partner.ImageId);

                partnerDto.Image = partnerImage;

                return partnerDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving partner by id: {exception.Message}");
            }
        }

        public async Task CreatePartner(CreateAndUpdatePartnerDTO partnerDto)
        {
            try
            {
                var partner = _mapper.Map<Partner>(partnerDto);
                var newImage = new CreateImageDTO
                {
                    Uri = partnerDto.Image,
                    AnimalId = null,
                };
                var imageId = await _imageService.AddImage(newImage);

                partner.ImageId = imageId;

                await _partnerRepository.AddPartner(partner);
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
                var existingPartner = await _partnerRepository.GetPartner(partnerId);
                var patchedPartner = _mapper.Map<CreateAndUpdatePartnerDTO>(existingPartner);
                var partnerImage = await _imageService.GetImageById(existingPartner.ImageId);

                patchedPartner.Image = partnerImage;

                patchDoc.ApplyTo(patchedPartner);

                await _imageService.UpdateImage(existingPartner.ImageId, patchedPartner.Image);

                existingPartner.Name = patchedPartner.Name;
                existingPartner.Uri = patchedPartner.Uri;

                await _partnerRepository.UpdatePartner(existingPartner);

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
                var removedPartner = await _partnerRepository.GetPartner(partnerId);

                await _partnerRepository.DeletePartner(removedPartner);
                await _imageService.DeleteImage(removedPartner.ImageId);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the partner: {exception.Message}");
            }
        }
    }
}
