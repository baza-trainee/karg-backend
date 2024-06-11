﻿using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<List<FAQDTO>> GetFAQs(string cultureCode)
        {
            try
            {
                var faqs = await _faqRepository.GetFAQs();
                var faqsDto = new List<FAQDTO>();

                foreach (var faq in faqs)
                {
                    var faqDto = new FAQDTO
                    {
                        Id = faq.Id,
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

        public async Task<FAQDTO> GetFAQById(int faqId, string cultureCode)
        {
            try
            {
                var faq = await _faqRepository.GetFAQ(faqId);
                var faqDto = _mapper.Map<FAQDTO>(faq);

                faqDto.Question = _localizationService.GetLocalizedValue(faq.Question, cultureCode, faq.QuestionId);
                faqDto.Answer = _localizationService.GetLocalizedValue(faq.Answer, cultureCode, faq.AnswerId);

                return faqDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving FAQ by id.", exception);
            }
        }

        public async Task<CreateAndUpdateFAQDTO> UpdateFAQ(int faqId, JsonPatchDocument<CreateAndUpdateFAQDTO> patchDoc)
        {
            try
            {
                var existingFAQ = await _faqRepository.GetFAQ(faqId);
                var patchedFAQ = _mapper.Map<CreateAndUpdateFAQDTO>(existingFAQ);

                patchedFAQ.Question_ua = _localizationService.GetLocalizedValue(existingFAQ.Question, "ua", existingFAQ.QuestionId);
                patchedFAQ.Question_en = _localizationService.GetLocalizedValue(existingFAQ.Question, "en", existingFAQ.QuestionId);
                patchedFAQ.Answer_ua = _localizationService.GetLocalizedValue(existingFAQ.Answer, "ua", existingFAQ.AnswerId);
                patchedFAQ.Answer_en = _localizationService.GetLocalizedValue(existingFAQ.Answer, "en", existingFAQ.AnswerId);

                patchDoc.ApplyTo(patchedFAQ);

                existingFAQ.QuestionId = await _localizationSetService.UpdateLocalizationSet(existingFAQ.QuestionId, patchedFAQ.Question_en, patchedFAQ.Question_ua);
                existingFAQ.AnswerId = await _localizationSetService.UpdateLocalizationSet(existingFAQ.AnswerId, patchedFAQ.Answer_en, patchedFAQ.Answer_ua);

                await _faqRepository.UpdateFAQ(existingFAQ);

                return patchedFAQ;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error updating the FAQ.", exception);
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
        
        public async Task DeleteFAQ(int faqId)
        {
            try
            {
                var removedFAQ = await _faqRepository.GetFAQ(faqId);
                var removedFAQQuestionId = removedFAQ.QuestionId;
                var removedFAQAnswerId = removedFAQ.AnswerId;

                await _faqRepository.DeleteFAQ(removedFAQ);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedFAQQuestionId, removedFAQAnswerId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error delete the FAQ.", exception);
            }
        }
    }
}
