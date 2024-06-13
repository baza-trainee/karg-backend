using karg.BLL.DTO.Partners;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Partners
{
    public interface IPartnerService
    {
        Task<List<PartnerDTO>> GetPartners();
        Task<PartnerDTO> GetPartnerById(int partnerId);
        Task CreatePartner(CreateAndUpdatePartnerDTO partnerDto);
        Task<CreateAndUpdatePartnerDTO> UpdatePartner(int partnerId, JsonPatchDocument<CreateAndUpdatePartnerDTO> patchDoc);
        Task DeletePartner(int partnerId);
    }
}
