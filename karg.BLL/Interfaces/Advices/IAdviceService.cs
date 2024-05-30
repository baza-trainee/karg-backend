using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Advices
{
    public interface IAdviceService
    {
        Task<PaginatedAllAdvicesDTO> GetAdvices(AdvicesFilterDTO filter, string cultureCode);
        Task CreateAdvice(CreateAndUpdateAdviceDTO adviceDto);
        Task<CreateAndUpdateAdviceDTO> UpdateAdvice(int adviceId, JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc);
        Task DeleteAdvice(int adviceId);
    }
}
