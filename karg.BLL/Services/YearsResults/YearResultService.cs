using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Utilities;
using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.YearsResults;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.YearsResults
{
    public class YearResultService : IYearResultService
    {
        private readonly IYearResultRepository _yearResultRepository;
        private readonly IPaginationService<YearResult> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;
        
        public YearResultService(IYearResultRepository yearResultRepository, IPaginationService<YearResult> paginationService, IImageService imageService, ILocalizationService localizationService, ILocalizationSetService localizationSetService, IMapper mapper)
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
                var paginatedYearsResultsItems = paginatedYearsResults.Items;
                var totalPages = paginatedYearsResults.TotalPages;
                var yearsResultsDto = new List<YearResultDTO>();

                foreach (var yearResult in paginatedYearsResultsItems)
                {
                    var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);
                    var yearResultImages = await _imageService.GetImagesByEntity("YearResult", yearResult.Id);

                    yearResultDto.Images = yearResultImages.Select(image => image.Uri).ToList();
                    yearResultDto.Description = _localizationService.GetLocalizedValue(yearResult.Description, cultureCode, yearResult.DescriptionId);
                    yearsResultsDto.Add(yearResultDto);
                }

                return new PaginatedAllYearsResultsDTO
                {
                    YearsResults = yearsResultsDto,
                    TotalPages = totalPages
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

                if(yearResult == null)
                {
                    return null;
                }

                var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);
                var yearResultImages = await _imageService.GetImagesByEntity("YearResult", yearResultId);

                yearResultDto.Description = _localizationService.GetLocalizedValue(yearResult.Description, cultureCode, yearResult.DescriptionId);
                yearResultDto.Images = yearResultImages.Select(image => image.Uri).ToList();

                return yearResultDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving year result by id: {exception.Message}");
            }
        }

        public async Task CreateYearResult(CreateAndUpdateYearResultDTO yearResultDto)
        {
            try
            {
                var yearResult = _mapper.Map<YearResult>(yearResultDto);

                yearResult.DescriptionId = await _localizationSetService.CreateAndSaveLocalizationSet(yearResultDto.Description_en, yearResultDto.Description_ua);

                var yearResultId = await _yearResultRepository.Add(yearResult);

                var newImages = yearResultDto.Images.Select(uri => new CreateImageDTO
                {
                    Uri = uri,
                    YearResultId = yearResultId
                }).ToList();

                await _imageService.AddImages(newImages);
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

                var yearResultImages = await _imageService.GetImagesByEntity("YearResult", yearResultId);
                patchedYearResult.Images = yearResultImages.Select(image => image.Uri).ToList();

                patchDoc.ApplyTo(patchedYearResult);

                await _imageService.UpdateEntityImages("YearResult", existingYearResult.Id, patchedYearResult.Images);

                existingYearResult.DescriptionId = await _localizationSetService.UpdateLocalizationSet(existingYearResult.DescriptionId, patchedYearResult.Description_en, patchedYearResult.Description_ua);
                existingYearResult.Year = new DateOnly(int.Parse(patchedYearResult.Year), 1, 1);

                await _yearResultRepository.Update(existingYearResult);

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
                var removedYearResultDescriptionId = removedYearResult.DescriptionId;

                await _imageService.DeleteImages("YearResult", removedYearResult.Id);
                await _yearResultRepository.Delete(removedYearResult);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedYearResultDescriptionId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the year result: {exception.Message}");
            }
        }
    }
}
