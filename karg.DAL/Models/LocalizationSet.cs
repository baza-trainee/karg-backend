﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Models
{
    public class LocalizationSet
    {
        public int Id { get; set; }

        public virtual ICollection<Localization> Localizations { get; set; }
    }
}
