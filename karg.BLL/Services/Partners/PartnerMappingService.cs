using karg.BLL.DTO.Partners;
using karg.BLL.Interfaces.Partners;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Partners
{
    public class PartnerMappingService : IPartnerMappingService
    {
        private readonly IImageService _imageService;

        public PartnerMappingService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<List<AllPartnersDTO>> MapToAllPartnersDTO(List<Partner> partners)
        {
            try
            {
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
                throw new ApplicationException("An error occurred while mapping partners to DTOs:", exception);
            }
        }
    }
}
