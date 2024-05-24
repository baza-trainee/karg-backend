using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Advices
{
    public class PaginatedAllAdvicesDTO
    {
        public List<AdviceDTO> Advices { get; set; }
        public int TotalPages { get; set; }
    }
}
