using AutoMapper;
using karg.BLL.DTO.Contacts;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;

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
    }
}
