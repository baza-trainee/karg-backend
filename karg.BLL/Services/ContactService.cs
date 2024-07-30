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
                var contactDTOs = _mapper.Map<List<ContactDTO>>(contacts);
                return contactDTOs;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving contacts: {exception.Message}");
            }
        }
    }
}
