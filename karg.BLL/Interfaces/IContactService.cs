using karg.BLL.DTO.Contacts;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactDTO>> GetContacts();
        Task<ContactDTO> GetContactById(int contactId);
        Task<UpdateContactDTO> UpdateContact(int contactId, JsonPatchDocument<UpdateContactDTO> patchDoc);
    }
}
