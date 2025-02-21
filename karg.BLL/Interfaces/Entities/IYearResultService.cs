using karg.BLL.DTO.YearsResults;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IYearResultService
    {
        Task<PaginatedAllYearsResultsDTO> GetYearsResults(YearsResultsFilterDTO filter, string cultureCode);
        Task<YearResultDTO> GetYearResultById(int yearResultId, string cultureCode);
        Task<CreateAndUpdateYearResultDTO> CreateYearResult(CreateAndUpdateYearResultDTO yearResultDto);
        Task<CreateAndUpdateYearResultDTO> UpdateYearResult(int yearResultId, JsonPatchDocument<CreateAndUpdateYearResultDTO> patchDoc);
        Task DeleteYearResult(int yearResultId);
    }
}
