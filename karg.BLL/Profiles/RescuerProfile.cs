using AutoMapper;
using karg.BLL.DTO.Rescuers;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Profiles
{
    public class RescuerProfile : Profile
    {
        public RescuerProfile() 
        {
            CreateMap<Rescuer, AllRescuersDTO>();
        }
    }
}
