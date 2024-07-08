using karg.BLL.DTO.YearsResults;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.YearsResults
{
    public interface IYearResultService
    {
        Task<PaginatedAllYearsResultsDTO> GetYearsResults(YearsResultsFilterDTO filter, string cultureCode);
        Task<YearResultDTO> GetYearResultById(int yearResultId, string cultureCode);
        Task CreateYearResult(CreateAndUpdateYearResultDTO yearResultDto);
        Task<CreateAndUpdateYearResultDTO> UpdateYearResult(int yearResultId, JsonPatchDocument<CreateAndUpdateYearResultDTO> patchDoc);
        Task DeleteYearResult(int yearResultId);
    }
}
