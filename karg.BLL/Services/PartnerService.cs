using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IImageService _imageService;

        public PartnerService(IPartnerRepository partnerRepository, IImageService imageService)
        {
            _partnerRepository = partnerRepository;
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
                    var partnerDto = new AllPartnersDTO
                    {
                        Name = partner.Name,
                        Image = partnerImage.Uri,
                        Uri = partner.Uri
                    };

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
