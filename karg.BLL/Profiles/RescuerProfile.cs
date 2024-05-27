using AutoMapper;
using karg.BLL.DTO.Rescuers;
using karg.DAL.Models;

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
