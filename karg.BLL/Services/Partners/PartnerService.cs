using AutoMapper;
using karg.BLL.DTO.Partners;
using karg.BLL.Interfaces.Partners;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<AllPartnersDTO>> GetPartners()
        {
            try
            {
                var partners = await _partnerRepository.GetPartners();
                var partnersDto = new List<AllPartnersDTO>();

                foreach (var partner in partners)
                {
                    var partnerImage = await _imageService.GetImageById(partner.ImageId);
                    var partnerDto = _mapper.Map<AllPartnersDTO>(partner);

                    partnerDto.Image = partnerImage.Uri;
                    partnersDto.Add(partnerDto);
                }

                return partnersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of partners.", exception);
            }
        }
    }
}
