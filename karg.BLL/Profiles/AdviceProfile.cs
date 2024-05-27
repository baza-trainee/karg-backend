using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class AdviceProfile : Profile
    {
        public AdviceProfile()
        {
            CreateMap<Advice, AdviceDTO>();
        }
    }
}
