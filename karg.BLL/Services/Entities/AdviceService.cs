using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Globalization;

namespace karg.BLL.Services.Entities
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceRepository _adviceRepository;
        private readonly IPaginationService<Advice> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;

        public AdviceService(
                IAdviceRepository adviceRepository,
                IPaginationService<Advice> paginationService,
                ILocalizationSetService localizationSetService,
                IImageService imageService,
                ILocalizationService localizationService,
                IMapper mapper)
        {
            _adviceRepository = adviceRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _localizationService = localizationService;
            _localizationSetService = localizationSetService;
            _mapper = mapper;
        }

        public async Task<PaginatedAllAdvicesDTO> GetAdvices(AdvicesFilterDTO filter, string cultureCode)
        {
            try
            {
                var advices = await _adviceRepository.GetAll();
                var paginatedAdvices = await _paginationService.PaginateWithTotalPages(advices, filter.Page, filter.PageSize);
                var advicesDto = new List<AdviceDTO>();

                foreach (var advice in paginatedAdvices.Items)
                {
                    var adviceDto = _mapper.Map<AdviceDTO>(advice);

                    adviceDto.Images = (await _imageService.GetImagesByEntity("Advice", advice.Id)).Select(image => image.Uri).ToList();
                    adviceDto.Description = _localizationService.GetLocalizedValue(advice.Description, cultureCode, advice.DescriptionId);
                    adviceDto.Title = _localizationService.GetLocalizedValue(advice.Title, cultureCode, advice.TitleId);
                    advicesDto.Add(adviceDto);
                }

                return new PaginatedAllAdvicesDTO
                {
                    Advices = advicesDto,
                    TotalPages = paginatedAdvices.TotalPages
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of advices: {exception.Message}");
            }
        }

        public async Task<AdviceDTO> GetAdviceById(int adviceId, string cultureCode)
        {
            try
            {
                var advice = await _adviceRepository.GetById(adviceId);
                if (advice == null) return null;

                var adviceDto = _mapper.Map<AdviceDTO>(advice);

                adviceDto.Images = (await _imageService.GetImagesByEntity("Advice", advice.Id)).Select(image => image.Uri).ToList();
                adviceDto.Title = _localizationService.GetLocalizedValue(advice.Title, cultureCode, advice.TitleId);
                adviceDto.Description = _localizationService.GetLocalizedValue(advice.Description, cultureCode, advice.DescriptionId);

                return adviceDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving advice by id: {exception.Message}");
            }
        }

        public async Task CreateAdvice(CreateAndUpdateAdviceDTO adviceDto)
        {
            try
            {
                var advice = _mapper.Map<Advice>(adviceDto);
                advice.TitleId = await _localizationSetService.CreateAndSaveLocalizationSet(adviceDto.Title_en, adviceDto.Title_ua);
                advice.DescriptionId = await _localizationSetService.CreateAndSaveLocalizationSet(adviceDto.Description_en, adviceDto.Description_ua);

                var adviceId = await _adviceRepository.Add(advice);

                var newImages = adviceDto.Images.Select(uri => new CreateImageDTO
                {
                    Uri = uri,
                    AdviceId = adviceId
                }).ToList();

                await _imageService.AddImages(newImages);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the advice: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateAdviceDTO> UpdateAdvice(int adviceId, JsonPatchDocument<CreateAndUpdateAdviceDTO> patchDoc)
        {
            try
            {
                var existingAdvice = await _adviceRepository.GetById(adviceId);
                var patchedAdvice = _mapper.Map<CreateAndUpdateAdviceDTO>(existingAdvice);

                patchedAdvice.Title_ua = _localizationService.GetLocalizedValue(existingAdvice.Title, "ua", existingAdvice.TitleId);
                patchedAdvice.Title_en = _localizationService.GetLocalizedValue(existingAdvice.Title, "en", existingAdvice.TitleId);
                patchedAdvice.Description_ua = _localizationService.GetLocalizedValue(existingAdvice.Description, "ua", existingAdvice.DescriptionId);
                patchedAdvice.Description_en = _localizationService.GetLocalizedValue(existingAdvice.Description, "en", existingAdvice.DescriptionId);
                patchedAdvice.Images = (await _imageService.GetImagesByEntity("Advice", adviceId)).Select(image => image.Uri).ToList();

                patchDoc.ApplyTo(patchedAdvice);

                await _imageService.UpdateEntityImages("Advice", adviceId, patchedAdvice.Images);

                existingAdvice.TitleId = await _localizationSetService.UpdateLocalizationSet(existingAdvice.TitleId, patchedAdvice.Title_en, patchedAdvice.Title_ua);
                existingAdvice.DescriptionId = await _localizationSetService.UpdateLocalizationSet(existingAdvice.DescriptionId, patchedAdvice.Description_en, patchedAdvice.Description_ua);
                existingAdvice.Created_At = DateOnly.ParseExact(patchedAdvice.Created_At, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                await _adviceRepository.Update(existingAdvice);

                return patchedAdvice;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the advice: {exception.Message}");
            }
        }

        public async Task DeleteAdvice(int adviceId)
        {
            try
            {
                var removedAdvice = await _adviceRepository.GetById(adviceId);

                await _imageService.DeleteImages("Advice", removedAdvice.Id);
                await _adviceRepository.Delete(removedAdvice);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedAdvice.TitleId, removedAdvice.DescriptionId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the advice: {exception.Message}");
            }
        }
    }
}