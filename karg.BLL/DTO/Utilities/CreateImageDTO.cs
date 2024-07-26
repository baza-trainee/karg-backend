using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Utilities
{
    public class CreateImageDTO
    {
        public Uri? Uri { get; set; }
        public int? AnimalId { get; set; }
        public int? AdviceId { get; set; }
        public int? RescuerId { get; set; }
        public int? PartnerId { get; set; }
        public int? YearResultId { get; set; }
    }
}
