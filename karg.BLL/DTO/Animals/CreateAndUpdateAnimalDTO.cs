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
        public string Category { get; set; }
        public string Name_en { get; set; }
        public string Description_en { get; set; }
        public string Story_en { get; set; }
        public string Name_ua { get; set; }
        public string Description_ua { get; set; }
        public string Story_ua { get; set; }
        public List<Uri> Images { get; set; }
    }
}
