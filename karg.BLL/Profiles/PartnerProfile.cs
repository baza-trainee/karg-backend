using AutoMapper;
using karg.BLL.DTO.Partners;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, PartnerDTO>();
            CreateMap<Partner, CreateAndUpdatePartnerDTO>();
            CreateMap<CreateAndUpdatePartnerDTO, Partner>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
