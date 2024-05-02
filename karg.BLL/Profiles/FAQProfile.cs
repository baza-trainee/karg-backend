using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Profiles
{
    public class FAQProfile : Profile
    {
        public FAQProfile() 
        { 
            CreateMap<FAQ, AllFAQsDTO>();
        }
    }
}
