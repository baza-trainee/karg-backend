using AutoMapper;
using karg.BLL.DTO.Partners;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, AllPartnersDTO>();
        }
    }
}
