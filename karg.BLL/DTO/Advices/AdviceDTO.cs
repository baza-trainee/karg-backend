using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Advices
{
    public class AdviceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Created_At { get; set; }
        public List<Uri> Images { get; set; }
    }
}
