using karg.BLL.DTO.Rescuers;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IRescuerService
    {
        Task<List<RescuerDTO>> GetRescuers();
        Task<RescuerDTO> GetRescuerById(int rescuerId);
        Task CreateRescuer(CreateAndUpdateRescuerDTO rescuerDto);
        Task<CreateAndUpdateRescuerDTO> UpdateRescuer(int rescuerId, JsonPatchDocument<CreateAndUpdateRescuerDTO> patchDoc);
        Task DeleteRescuer(int rescuerId);
    }
}
