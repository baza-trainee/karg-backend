using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Profiles
{
    public class AdviceProfile : Profile
    {
        public AdviceProfile()
        {
            CreateMap<Advice, AdviceDTO>();
        }
    }
}
