using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            CreateMap<FAQ, FAQDTO>();
            CreateMap<FAQ, CreateAndUpdateFAQDTO>();
            CreateMap<CreateAndUpdateFAQDTO, FAQ>()
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.Answer, opt => opt.Ignore());
        }
    }
}
