using AutoMapper;
using karg.BLL.DTO.Contacts;
using karg.BLL.Interfaces.Entities;
using karg.DAL.Interfaces;
using karg.DAL.Models.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<List<ContactDTO>> GetContacts()
        {
            try
            {
                var contacts = await _contactRepository.GetAll();
                return _mapper.Map<List<ContactDTO>>(contacts);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving contacts: {exception.Message}");
            }
        }

        public async Task<UpdateContactDTO> UpdateContact(int contactId, JsonPatchDocument<UpdateContactDTO> patchDoc)
        {
            try
            {
                var existingContact = await _contactRepository.GetById(contactId);
                var patchedContact = _mapper.Map<UpdateContactDTO>(existingContact);

                patchDoc.ApplyTo(patchedContact);

                existingContact.Category = Enum.Parse<ContactCategory>(patchedContact.Category);
                existingContact.Value = patchedContact.Value;

                await _contactRepository.Update(existingContact);

                return patchedContact;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the FAQ: {exception.Message}");
            }
        }
    }
}
