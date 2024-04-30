using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Animals
{
    public class AnimalsFilterDTO
    {
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;
        [DefaultValue(6)]
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50.")]
        public int PageSize { get; set; } = 6;
        public string? CategoryFilter { get; set; } = null;
        public string? NameSearch { get; set; } = null;
    }
}
