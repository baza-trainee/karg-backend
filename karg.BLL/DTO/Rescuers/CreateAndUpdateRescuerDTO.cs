using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Rescuers
{
    public class CreateAndUpdateRescuerDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public Uri Image { get; set; }
    }
}
