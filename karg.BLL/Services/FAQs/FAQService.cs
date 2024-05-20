using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Utilities;
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
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public FAQService(IFAQRepository faqRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public async Task<List<AllFAQsDTO>> GetFAQs(string cultureCode)
        {
            try
            {
                var faqs = await _faqRepository.GetFAQs();
                var faqsDto = new List<AllFAQsDTO>();

                foreach (var faq in faqs)
                {
                    var faqDto = new AllFAQsDTO
                    {
                        Answer = _localizationService.GetLocalizedValue(faq.Answer, cultureCode, faq.AnswerId),
                        Question = _localizationService.GetLocalizedValue(faq.Question, cultureCode, faq.QuestionId)
                    };

                    faqsDto.Add(faqDto);
                }

                return faqsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of FAQs.", exception);
            }
        }
    }
}
