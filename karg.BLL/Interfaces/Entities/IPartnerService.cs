using karg.BLL.DTO.Partners;
using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IPartnerService
    {
        Task<PaginatedResult<PartnerDTO>> GetPartners(PartnerFilterDTO filter);
        Task<PartnerDTO> GetPartnerById(int partnerId);
        Task<CreateAndUpdatePartnerDTO> CreatePartner(CreateAndUpdatePartnerDTO partnerDto);
        Task<CreateAndUpdatePartnerDTO> UpdatePartner(int partnerId, JsonPatchDocument<CreateAndUpdatePartnerDTO> patchDoc);
        Task DeletePartner(int partnerId);
    }
}
