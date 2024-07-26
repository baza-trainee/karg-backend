using AutoMapper;
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
                var partners = await _partnerRepository.GetAll();
                var partnersDto = new List<PartnerDTO>();

                foreach (var partner in partners)
                {
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);
                    var partnerImages = await _imageService.GetImagesByEntity("Partner", partner.Id);

                    partnerDto.Images = partnerImages.Select(image => image.Uri).ToList();
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
                
                if(partner == null)
                {
                    return null;
                }

                var partnerDto = _mapper.Map<PartnerDTO>(partner);
                var partnerImages = await _imageService.GetImagesByEntity("Partner", partner.Id);

                partnerDto.Images = partnerImages.Select(image => image.Uri).ToList();

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
                var partnerId = await _partnerRepository.Add(partner);

                var newImages = partnerDto.Images.Select(uri => new CreateImageDTO
                {
                    Uri = uri,
                    PartnerId = partnerId
                }).ToList();

                await _imageService.AddImages(newImages);
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
                var partnerImages = await _imageService.GetImagesByEntity("Partner", partnerId);

                patchedPartner.Images = partnerImages.Select(image => image.Uri).ToList();
                patchDoc.ApplyTo(patchedPartner);

                await _imageService.UpdateEntityImages("Partner", partnerId, patchedPartner.Images);

                existingPartner.Name = patchedPartner.Name;
                existingPartner.Uri = patchedPartner.Uri;

                await _partnerRepository.Update(existingPartner);

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
