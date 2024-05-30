using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class AdviceProfile : Profile
    {
        public AdviceProfile()
        {
            CreateMap<Advice, CreateAndUpdateAdviceDTO>();
            CreateMap<Advice, AdviceDTO>();
            CreateMap<CreateAndUpdateAdviceDTO, Advice>()
                .ForMember(dest => dest.Created_At, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Created_At, "yyyy-MM-dd")))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());
        }
    }
}
