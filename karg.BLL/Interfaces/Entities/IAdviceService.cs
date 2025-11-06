using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IAdviceService
    {
        Task<PaginatedResult<AdviceDTO>> GetAdvices(AdvicesFilterDTO filter, string cultureCode);
        Task<CreateAndUpdateAdviceDTO> CreateAdvice(CreateAndUpdateAdviceDTO adviceDto);
        Task<AdviceDTO> GetAdviceById(int adviceId, string cultureCode);
        Task<CreateAndUpdateAdviceDTO> UpdateAdvice(int adviceId, JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc);
        Task DeleteAdvice(int adviceId);
    }
}
