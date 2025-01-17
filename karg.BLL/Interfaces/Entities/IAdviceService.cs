using karg.BLL.DTO.Advices;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IAdviceService
    {
        Task<PaginatedAllAdvicesDTO> GetAdvices(AdvicesFilterDTO filter, string cultureCode);
        Task CreateAdvice(CreateAndUpdateAdviceDTO adviceDto);
        Task<AdviceDTO> GetAdviceById(int adviceId, string cultureCode);
        Task<CreateAndUpdateAdviceDTO> UpdateAdvice(int adviceId, JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc);
        Task DeleteAdvice(int adviceId);
    }
}
