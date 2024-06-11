using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Partners
{
    public class PartnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public Uri Image { get; set; }
    }
}
