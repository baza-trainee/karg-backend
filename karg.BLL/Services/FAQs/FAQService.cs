using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.FAQs
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;
        private readonly IFAQMappingService _faqMappingService;

        public FAQService(IFAQRepository faqRepository, IFAQMappingService faqMappingService)
        {
            _faqRepository = faqRepository;
            _faqMappingService = faqMappingService;
        }

        public async Task<List<AllFAQsDTO>> GetFAQs()
        {
            try
            {
                var faqs = await _faqRepository.GetFAQs();
                var faqsDto = await _faqMappingService.MapToAllFAQsDTO(faqs);

                return faqsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of FAQs.", exception);
            }
        }
    }
}
