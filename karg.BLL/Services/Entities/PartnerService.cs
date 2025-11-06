using AutoMapper;
using karg.BLL.DTO.Partners;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IPaginationService<Partner> _paginationService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public PartnerService(
            IPartnerRepository partnerRepository,
            IPaginationService<Partner> paginationService,
            IMapper mapper,
            IImageService imageService)
        {
            _partnerRepository = partnerRepository;
            _paginationService = paginationService;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<PaginatedResult<PartnerDTO>> GetPartners(PartnerFilterDTO filter)
        {
            try
            {
                var partners = await _partnerRepository.GetAll();
                var paginatedPartners = await _paginationService.PaginateWithTotalPages(partners, filter.Page, filter.PageSize);
                var partnersDto = new List<PartnerDTO>();

                foreach (var partner in paginatedPartners.Items)
                {
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);

                    partnerDto.Images = (await _imageService.GetImagesByEntity("Partner", partner.Id)).Select(image => image.Uri).ToList();
                    partnersDto.Add(partnerDto);
                }

                return new PaginatedResult<PartnerDTO>
                {
                    Items = partnersDto,
                    TotalPages = paginatedPartners.TotalPages,
                    TotalItems = partners.Count()
                };
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

                partnerDto.Images = await _imageService.UploadImages(nameof(Partner), partnerId, partnerDto.Images, false);

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
                    patchedPartner.Images = await _imageService.UploadImages(nameof(Partner), partnerId, patchedPartner.Images, true);
                }

                existingPartner.Name = patchedPartner.Name;
                existingPartner.Uri = new Uri(patchedPartner.Uri);

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
