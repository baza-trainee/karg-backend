using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.Animals
{
    public class AnimalsFilterDTO
    {
        [FromQuery(Name = "page")]
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [DefaultValue(6)]
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50.")]
        public int PageSize { get; set; } = 6;

        [FromQuery(Name = "categoryFilter")]
        public string? CategoryFilter { get; set; } = null;

        [FromQuery(Name = "nameSearch")]
        public string? NameSearch { get; set; } = null;
    }
}