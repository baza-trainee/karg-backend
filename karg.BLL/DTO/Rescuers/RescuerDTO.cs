using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Rescuers
{
    public class RescuerDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {  get; set; }
        public List<Uri>? Images { get; set; }
    }
}
