using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO
{
    public class AnimalsFilterDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? CategoryFilter { get; set; } = null;
        public string? NameSearch { get; set; } = null;
    }
}
