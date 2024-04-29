using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<List<AllFAQsDTO>> GetFAQs()
        {
            try
            {
                var faqs = await _faqRepository.GetFAQs();
                var faqsDto = faqs.Select(faq => new AllFAQsDTO
                {
                    Question = faq.Question,
                    Answer = faq.Answer
                }).ToList();

                return faqsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of FAQs.", exception);
            }
        }
    }
}
