using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
using karg.DAL.Models;
using System.Globalization;

namespace karg.BLL.Profiles
{
    public class AdviceProfile : Profile
    {
        public AdviceProfile()
        {
            CreateMap<Advice, CreateAndUpdateAdviceDTO>()
                .ForMember(dest => dest.Created_At, opt => opt.MapFrom(src => src.Created_At.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            CreateMap<Advice, AdviceDTO>();
            CreateMap<CreateAndUpdateAdviceDTO, Advice>()
                .ForMember(dest => dest.Created_At, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Created_At, "yyyy-MM-dd")))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());
        }
    }
}
