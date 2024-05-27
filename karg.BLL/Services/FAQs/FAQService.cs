using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.BLL.Services.FAQs
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;

        public FAQService(IFAQRepository faqRepository, ILocalizationService localizationService, ILocalizationSetService localizationSetService, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _localizationService = localizationService;
            _localizationSetService = localizationSetService;
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

        public async Task CreateFAQ(CreateAndUpdateFAQDTO faqDto)
        {
            try
            {
                var faq = _mapper.Map<FAQ>(faqDto);

                faq.QuestionId = await _localizationSetService.CreateAndSaveLocalizationSet(faqDto.Question_en, faqDto.Question_ua);
                faq.AnswerId = await _localizationSetService.CreateAndSaveLocalizationSet(faqDto.Answer_en, faqDto.Answer_ua);

                await _faqRepository.AddFAQ(faq);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error adding the FAQ.", exception);
            }
        }
    }
}
