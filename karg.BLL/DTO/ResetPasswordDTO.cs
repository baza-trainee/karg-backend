using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO
{
    public class ResetPasswordDTO
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
