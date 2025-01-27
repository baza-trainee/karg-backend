using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace karg.BLL.DTO.YearsResults
{
    public class YearsResultsFilterDTO
    {
        [FromQuery(Name = "page")]
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [DefaultValue(6)]
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50.")]
        public int PageSize { get; set; } = 6;
    }
}