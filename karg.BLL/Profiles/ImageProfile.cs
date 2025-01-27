using AutoMapper;
using karg.BLL.DTO.Utilities;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile() 
        {
            CreateMap<CreateImageDTO, Image>();
            CreateMap<Image, CreateImageDTO>();
        }
    }
}
