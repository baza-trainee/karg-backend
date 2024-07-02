using AutoMapper;
using karg.BLL.DTO.Authentication;
using karg.DAL.Models;

namespace karg.BLL.Profiles
{
    public class JwtTokenProfile : Profile
    {
        public JwtTokenProfile()
        {
            CreateMap<JwtToken, RescuerJwtTokenDTO>();
        }
    }
}
