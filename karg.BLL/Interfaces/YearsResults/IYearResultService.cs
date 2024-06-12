using karg.BLL.DTO.YearsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.YearsResults
{
    public interface IYearResultService
    {
        Task<PaginatedAllYearsResultsDTO> GetYearsResults(YearsResultsFilterDTO filter, string cultureCode);
        Task<YearResultDTO> GetYearResultById(int yearResultId, string cultureCode);
    }
}
