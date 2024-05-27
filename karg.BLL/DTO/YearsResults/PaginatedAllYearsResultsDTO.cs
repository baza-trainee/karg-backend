using karg.BLL.DTO.Advices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.YearsResults
{
    public class PaginatedAllYearsResultsDTO
    {
        public List<YearResultDTO> YearsResults { get; set; }
        public int TotalPages { get; set; }
    }
}
