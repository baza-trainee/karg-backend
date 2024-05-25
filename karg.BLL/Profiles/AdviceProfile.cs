using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
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
            CreateMap<CreateAndUpdateAdviceDTO, Advice>()
                .ForMember(dest => dest.Created_At, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Created_At, "yyyy-MM-dd")))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());
        }
    }
}
