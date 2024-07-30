using AutoMapper;
using karg.DAL.Models;
using karg.BLL.DTO.Contacts;

namespace karg.BLL.Profiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile() 
        {
            CreateMap<Contact, ContactDTO>();
        }
    }
}
