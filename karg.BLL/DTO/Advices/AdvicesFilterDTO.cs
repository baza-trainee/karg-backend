using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace karg.BLL.DTO.Advices
{
    public class AdvicesFilterDTO : PaginationFilter 
    {
        [FromQuery(Name = "nameSearch")]
        public string? NameSearch { get; set; } = null;
    }
}