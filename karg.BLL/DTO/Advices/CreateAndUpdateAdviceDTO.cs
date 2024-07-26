using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Advices
{
    public class CreateAndUpdateAdviceDTO
    {
        public string Title_en { get; set; }
        public string Title_ua { get; set; }
        public string Description_en { get; set; }
        public string Description_ua { get; set; }
        public string Created_At { get; set; }
        public List<Uri> Images { get; set; }
    }
}
