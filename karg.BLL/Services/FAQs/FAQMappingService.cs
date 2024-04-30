using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.FAQs
{
    public class FAQMappingService : IFAQMappingService
    {
        public async Task<List<AllFAQsDTO>> MapToAllFAQsDTO(List<FAQ> faqs)
        {
            var faqsDto = faqs.Select(faq => new AllFAQsDTO
            {
                Question = faq.Question,
                Answer = faq.Answer
            }).ToList();

            return faqsDto;
        }
    }
}
