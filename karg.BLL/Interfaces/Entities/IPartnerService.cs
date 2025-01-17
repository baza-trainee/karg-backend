using karg.BLL.DTO.Partners;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
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
