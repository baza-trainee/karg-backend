using AutoMapper;
using karg.BLL.DTO.Utilities;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile() 
        {
            CreateMap<CreateImageDTO, Image>()
                .ForMember(dest => dest.BinaryData, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Base64Data) ? Convert.FromBase64String(src.Base64Data) : null));

            CreateMap<Image, CreateImageDTO>()
                .ForMember(dest => dest.Base64Data, opt => opt.MapFrom(src =>
                    src.BinaryData != null ? Convert.ToBase64String(src.BinaryData) : null));
        }
    }
}
