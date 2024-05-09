using karg.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.Animals
{
    public class CreateAndUpdateAnimalDTO
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Short_Description { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public List<Uri> Images { get; set; }
    }
}
