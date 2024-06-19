using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.YearsResults
{
    public class CreateAndUpdateYearResultDTO
    {
        public string Description_en { get; set; }
        public string Description_ua { get; set; }
        public string Year { get; set; }
        public Uri Image { get; set; }
    }
}
