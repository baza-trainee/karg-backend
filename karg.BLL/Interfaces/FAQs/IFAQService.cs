using karg.BLL.DTO.FAQs;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.FAQs
{
    public interface IFAQService
    {
        Task<List<AllFAQsDTO>> GetFAQs(string cultureCode);
        Task CreateFAQ(CreateAndUpdateFAQDTO faqDto);
        Task<CreateAndUpdateFAQDTO> UpdateFAQ(int faqId, JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc);
        Task DeleteFAQ(int faqId);
    }
}
