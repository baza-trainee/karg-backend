using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Models
{
    public class JwtToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int RescuerId { get; set; }
        public Rescuer? Rescuer { get; set; }
    }
}
