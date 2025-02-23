using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace karg.BLL.DTO.Animals
{
    public class AnimalsFilterDTO : PaginationFilterDTO
    {
        [FromQuery(Name = "categoryFilter")]
        public string? CategoryFilter { get; set; } = null;

        [FromQuery(Name = "nameSearch")]
        public string? NameSearch { get; set; } = null;
    }
}