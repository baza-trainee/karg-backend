using karg.BLL.DTO.FAQs;
using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IFAQService
    {
        Task<PaginatedResult<FAQDTO>> GetFAQs(FAQsFilterDTO filter, string cultureCode);
        Task<FAQDTO> GetFAQById(int faqId, string cultureCode);
        Task CreateFAQ(CreateAndUpdateFAQDTO faqDto);
        Task<CreateAndUpdateFAQDTO> UpdateFAQ(int faqId, JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc);
        Task DeleteFAQ(int faqId);
    }
}
