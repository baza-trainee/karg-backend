using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace karg.BLL.DTO.YearsResults
{
    public class YearsResultsFilterDTO : PaginationFilter 
    {
        [FromQuery(Name = "shortVersion")]
        public bool ShortVersion { get; set; } = false;
    }
}