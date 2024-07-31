using AutoMapper;
using karg.BLL.DTO.Contacts;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using karg.DAL.Models.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services
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
                var contactsDto = _mapper.Map<List<ContactDTO>>(contacts);
                return contactsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving contacts: {exception.Message}");
            }
        }

        public async Task<ContactDTO> GetContactById(int contactId)
        {
            try
            {
                var contact = await _contactRepository.GetById(contactId);
                var contactDTO = _mapper.Map<ContactDTO>(contact);
                return contactDTO;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving contact by id: {exception.Message}");
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
