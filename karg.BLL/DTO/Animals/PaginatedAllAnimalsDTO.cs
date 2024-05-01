using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Animals
{
    public class PaginatedAnimalsDTO
    {
        public List<AllAnimalsDTO> Animals { get; set; }
        public int TotalPages { get; set; }
    }
}
