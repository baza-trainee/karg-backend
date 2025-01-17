using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;

        public FAQService(
            IFAQRepository faqRepository,
            ILocalizationService localizationService,
            ILocalizationSetService localizationSetService,
            IMapper mapper)
        {
            _faqRepository = faqRepository;
            _localizationService = localizationService;
            _localizationSetService = localizationSetService;
            _mapper = mapper;
        }

        public async Task<List<FAQDTO>> GetFAQs(string cultureCode)
        {
            try
            {
                var faqs = await _faqRepository.GetAll();
                var faqsDto = faqs.Select(faq => new FAQDTO
                {
                    Id = faq.Id,
                    Answer = _localizationService.GetLocalizedValue(faq.Answer, cultureCode, faq.AnswerId),
                    Question = _localizationService.GetLocalizedValue(faq.Question, cultureCode, faq.QuestionId)
                }).ToList();

                return faqsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of FAQs: {exception.Message}");
            }
        }

        public async Task<FAQDTO> GetFAQById(int faqId, string cultureCode)
        {
            try
            {
                var faq = await _faqRepository.GetById(faqId);
                if (faq == null) return null;

                var faqDto = _mapper.Map<FAQDTO>(faq);
                faqDto.Question = _localizationService.GetLocalizedValue(faq.Question, cultureCode, faq.QuestionId);
                faqDto.Answer = _localizationService.GetLocalizedValue(faq.Answer, cultureCode, faq.AnswerId);

                return faqDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving FAQ by id: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateFAQDTO> UpdateFAQ(int faqId, JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc)
        {
            try
            {
                var existingFAQ = await _faqRepository.GetById(faqId);
                var patchedFAQ = _mapper.Map<CreateAndUpdateFAQDTO>(existingFAQ);

                patchedFAQ.Question_ua = _localizationService.GetLocalizedValue(existingFAQ.Question, "ua", existingFAQ.QuestionId);
                patchedFAQ.Question_en = _localizationService.GetLocalizedValue(existingFAQ.Question, "en", existingFAQ.QuestionId);
                patchedFAQ.Answer_ua = _localizationService.GetLocalizedValue(existingFAQ.Answer, "ua", existingFAQ.AnswerId);
                patchedFAQ.Answer_en = _localizationService.GetLocalizedValue(existingFAQ.Answer, "en", existingFAQ.AnswerId);

                patchDoc.ApplyTo(patchedFAQ);

                existingFAQ.QuestionId = await _localizationSetService.UpdateLocalizationSet(existingFAQ.QuestionId, patchedFAQ.Question_en, patchedFAQ.Question_ua);
                existingFAQ.AnswerId = await _localizationSetService.UpdateLocalizationSet(existingFAQ.AnswerId, patchedFAQ.Answer_en, patchedFAQ.Answer_ua);

                await _faqRepository.Update(existingFAQ);

                return patchedFAQ;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the FAQ: {exception.Message}");
            }
        }

        public async Task CreateFAQ(CreateAndUpdateFAQDTO faqDto)
        {
            try
            {
                var faq = _mapper.Map<FAQ>(faqDto);
                faq.QuestionId = await _localizationSetService.CreateAndSaveLocalizationSet(faqDto.Question_en, faqDto.Question_ua);
                faq.AnswerId = await _localizationSetService.CreateAndSaveLocalizationSet(faqDto.Answer_en, faqDto.Answer_ua);

                await _faqRepository.Add(faq);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the FAQ: {exception.Message}");
            }
        }

        public async Task DeleteFAQ(int faqId)
        {
            try
            {
                var removedFAQ = await _faqRepository.GetById(faqId);

                await _faqRepository.Delete(removedFAQ);
                await _localizationSetService.DeleteLocalizationSets(new List<int>
                {
                    removedFAQ.QuestionId,
                    removedFAQ.AnswerId
                });
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the FAQ: {exception.Message}");
            }
        }
    }
}
