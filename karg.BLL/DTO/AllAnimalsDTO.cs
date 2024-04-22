using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO
{
    public class AllAnimalsDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Story { get; set; }
        public Uri Image { get; set; }
    }
}
