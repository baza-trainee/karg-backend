using AutoMapper;
using karg.BLL.DTO.YearsResults;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class YearResultProfile : Profile
    {
        public YearResultProfile()
        {
            CreateMap<YearResult, CreateAndUpdateYearResultDTO>();
            CreateMap<YearResult, YearResultDTO>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year.Year.ToString()));
            CreateMap<CreateAndUpdateYearResultDTO, YearResult>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => new DateOnly(int.Parse(src.Year), 1, 1)))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());
        }
    }
}
