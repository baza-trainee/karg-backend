using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Models
{
    public class Localization
    {
        public int LocalizationSetId { get; set; }
        public string CultureCode { get; set; }
        public string Value { get; set; }

        public virtual LocalizationSet LocalizationSet { get; set; }
        public virtual Culture Culture { get; set; }
    }
}
