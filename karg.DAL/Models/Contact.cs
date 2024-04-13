using karg.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public ContactCategory Category { get; set; }
        public Uri? Uri { get; set; }
    }
}
