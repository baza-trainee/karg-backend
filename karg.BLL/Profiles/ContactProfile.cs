using AutoMapper;
using karg.DAL.Models;
using karg.BLL.DTO.Contacts;
using karg.DAL.Models.Enums;

namespace karg.BLL.Profiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile() 
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<Contact, UpdateContactDTO>();
            CreateMap<UpdateContactDTO, Contact>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse(typeof(ContactCategory), src.Category)));
        }
    }
}
