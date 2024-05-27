using AutoMapper;
using karg.BLL.DTO.YearsResults;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class YearResultProfile : Profile
    {
        public YearResultProfile()
        {
            CreateMap<YearResult, YearResultDTO>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year.Year.ToString()));
        }
    }
}
