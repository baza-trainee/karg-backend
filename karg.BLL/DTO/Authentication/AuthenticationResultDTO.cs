using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Authentication
{
    public class AuthenticationResultDTO
    {
        public string? Token { get; set; }
        public int Status { get; set; }
        public int RescuerId { get; set; }
        public string Message { get; set; }
    }
}
