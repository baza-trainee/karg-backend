using AutoMapper;
using karg.BLL.DTO.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile() 
        {
            CreateMap<CreateImageDTO, Image>();
        }
    }
}
