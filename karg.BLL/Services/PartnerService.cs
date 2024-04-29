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
        private readonly IPartnerMappingService _partnerMappingService;

        public PartnerService(IPartnerRepository partnerRepository, IPartnerMappingService partnerMappingService)
        {
            _partnerRepository = partnerRepository;
            _partnerMappingService = partnerMappingService;
        }

        public async Task<List<AllPartnersDTO>> GetPartners()
        {
            try
            {
                var partners = await _partnerRepository.GetPartners();
                var partnersDto = await _partnerMappingService.MapToAllPartnersDTO(partners);

                return partnersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of partners.", exception);
            }
        }
    }
}
