using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IYearResultRepository
    {
        Task<List<YearResult>> GetYearsResults();
        Task<YearResult> GetYearResult(int yearResultId);
        Task<int> AddYearResult(YearResult yearResult);
        Task DeleteYearResult(YearResult yearResult);
    }
}
