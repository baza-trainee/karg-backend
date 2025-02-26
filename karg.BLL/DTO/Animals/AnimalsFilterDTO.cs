using karg.BLL.DTO.Utilities;
using karg.DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace karg.BLL.DTO.Animals
{
    public class AnimalsFilterDTO : PaginationFilterDTO
    {
        [FromQuery(Name = "categoryFilter")]
        public string? CategoryFilter { get; set; } = null;

        [FromQuery(Name = "nameSearch")]
        public string? NameSearch { get; set; } = null;

        [FromQuery(Name = "sortOrder")]
        public AnimalSortOrder SortOrder { get; set; } = AnimalSortOrder.Latest;
    }
}