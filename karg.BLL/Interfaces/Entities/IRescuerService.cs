using karg.BLL.DTO.Rescuers;
using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IRescuerService
    {
        Task<PaginatedResult<RescuerDTO>> GetRescuers(RescuersFilterDTO filter);
        Task<RescuerDTO> GetRescuerById(int rescuerId);
        Task<CreateAndUpdateRescuerDTO> CreateRescuer(CreateAndUpdateRescuerDTO rescuerDto);
        Task<CreateAndUpdateRescuerDTO> UpdateRescuer(int rescuerId, JsonPatchDocument<CreateAndUpdateRescuerDTO> patchDoc);
        Task DeleteRescuer(int rescuerId);
    }
}
