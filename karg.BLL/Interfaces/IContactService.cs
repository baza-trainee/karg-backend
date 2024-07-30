using karg.BLL.DTO.Contacts;

namespace karg.BLL.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactDTO>> GetContacts();
    }
}
