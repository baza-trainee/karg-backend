using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace karg.BLL.DTO.FAQs
{
    public class FAQsFilterDTO : PaginationFilterDTO 
    {
        [FromQuery(Name = "pageSize")]
        [DefaultValue(10)]
        public override int PageSize { get; set; } = 10;
    }
}
