﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.DTO.FAQs
{
    public class CreateAndUpdateFAQDTO
    {
        public string Question_en { get; set; }
        public string Question_ua { get; set; }
        public string Answer_en { get; set; }
        public string Answer_ua { get; set; }
    }
}
