using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.YearsResults
{
    public class YearResultDTO
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public List<Uri> Images { get; set; }
    }
}
