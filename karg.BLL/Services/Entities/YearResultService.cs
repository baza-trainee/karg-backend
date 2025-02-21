using AutoMapper;
using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Partners;
using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class YearResultService : IYearResultService
    {
        private readonly IYearResultRepository _yearResultRepository;
        private readonly IPaginationService<YearResult> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;

        public YearResultService(
            IYearResultRepository yearResultRepository, 
            IPaginationService<YearResult> paginationService, 
            IImageService imageService, 
            ILocalizationService localizationService, 
            ILocalizationSetService localizationSetService, 
            IMapper mapper)
        {
            _yearResultRepository = yearResultRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _localizationService = localizationService;
            _localizationSetService = localizationSetService;
            _mapper = mapper;
        }

        public async Task<PaginatedAllYearsResultsDTO> GetYearsResults(YearsResultsFilterDTO filter, string cultureCode)
        {
            try
            {
                var yearsResults = await _yearResultRepository.GetAll();
                var paginatedYearsResults = await _paginationService.PaginateWithTotalPages(yearsResults, filter.Page, filter.PageSize);
                var yearsResultsDto = new List<YearResultDTO>();

                foreach (var yearResult in paginatedYearsResults.Items)
                {
                    var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);

                    yearResultDto.Images = (await _imageService.GetImagesByEntity("YearResult", yearResult.Id)).Select(image => image.Uri).ToList();
                    yearResultDto.Description = _localizationService.GetLocalizedValue(yearResult.Description, cultureCode, yearResult.DescriptionId);
                    yearsResultsDto.Add(yearResultDto);
                }

                return new PaginatedAllYearsResultsDTO
                {
                    YearsResults = yearsResultsDto,
                    TotalPages = paginatedYearsResults.TotalPages
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of years results: {exception.Message}");
            }
        }

        public async Task<YearResultDTO> GetYearResultById(int yearResultId, string cultureCode)
        {
            try
            {
                var yearResult = await _yearResultRepository.GetById(yearResultId);
                if (yearResult == null) return null;

                var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);
                yearResultDto.Description = _localizationService.GetLocalizedValue(yearResult.Description, cultureCode, yearResult.DescriptionId);
                yearResultDto.Images = (await _imageService.GetImagesByEntity("YearResult", yearResultId)).Select(image => image.Uri).ToList();

                return yearResultDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving year result by id: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateYearResultDTO> CreateYearResult(CreateAndUpdateYearResultDTO yearResultDto)
        {
            try
            {
                var yearResult = _mapper.Map<YearResult>(yearResultDto);
                yearResult.DescriptionId = await _localizationSetService.CreateAndSaveLocalizationSet(yearResultDto.Description_en, yearResultDto.Description_ua);

                var yearResultId = await _yearResultRepository.Add(yearResult);

                if (yearResultDto.Images != null && yearResultDto.Images.Any())
                {
                    var imageBytesList = yearResultDto.Images
                        .Select(Convert.FromBase64String)
                        .ToList();

                    await _imageService.AddImages(nameof(YearResult), yearResultId, imageBytesList);
                }

                yearResultDto.Images = (await _imageService.GetImagesByEntity(nameof(YearResult), yearResultId)).Select(image => image.Uri).ToList();

                return yearResultDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the year result: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateYearResultDTO> UpdateYearResult(int yearResultId, JsonPatchDocument<CreateAndUpdateYearResultDTO> patchDoc)
        {
            try
            {
                var existingYearResult = await _yearResultRepository.GetById(yearResultId);
                var patchedYearResult = _mapper.Map<CreateAndUpdateYearResultDTO>(existingYearResult);

                patchedYearResult.Description_ua = _localizationService.GetLocalizedValue(existingYearResult.Description, "ua", existingYearResult.DescriptionId);
                patchedYearResult.Description_en = _localizationService.GetLocalizedValue(existingYearResult.Description, "en", existingYearResult.DescriptionId);
                patchedYearResult.Year = existingYearResult.Year.Year.ToString();

                patchDoc.ApplyTo(patchedYearResult);

                if (patchDoc.Operations.Any(op => op.path == "/images"))
                {
                    var imageBytesList = patchedYearResult.Images
                                .Select(Convert.FromBase64String)
                                .ToList();
                    await _imageService.UpdateEntityImages(nameof(YearResult), yearResultId, imageBytesList);
                }

                existingYearResult.DescriptionId = await _localizationSetService.UpdateLocalizationSet(existingYearResult.DescriptionId, patchedYearResult.Description_en, patchedYearResult.Description_ua);
                existingYearResult.Year = new DateOnly(int.Parse(patchedYearResult.Year), 1, 1);

                await _yearResultRepository.Update(existingYearResult);
                patchedYearResult.Images = (await _imageService.GetImagesByEntity(nameof(YearResult), yearResultId)).Select(image => image.Uri).ToList();

                return patchedYearResult;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the year result: {exception.Message}");
            }
        }

        public async Task DeleteYearResult(int yearResultId)
        {
            try
            {
                var removedYearResult = await _yearResultRepository.GetById(yearResultId);

                await _imageService.DeleteImages("YearResult", removedYearResult.Id);
                await _yearResultRepository.Delete(removedYearResult);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedYearResult.DescriptionId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the year result: {exception.Message}");
            }
        }
    }
}
