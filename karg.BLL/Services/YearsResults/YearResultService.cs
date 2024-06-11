using AutoMapper;
using karg.BLL.DTO.YearsResults;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Interfaces.YearsResults;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.BLL.Services.YearsResults
{
    public class YearResultService : IYearResultService
    {
        private readonly IYearResultRepository _yearResultRepository;
        private readonly IPaginationService<YearResult> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        public YearResultService(IYearResultRepository yearResultRepository, IPaginationService<YearResult> paginationService, IImageService imageService, ILocalizationService localizationService, IMapper mapper)
        {
            _yearResultRepository = yearResultRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public async Task<PaginatedAllYearsResultsDTO> GetYearsResults(YearsResultsFilterDTO filter, string cultureCode)
        {
            try
            {
                var yearsResults = await _yearResultRepository.GetYearsResults();
                var paginatedYearsResults = await _paginationService.PaginateWithTotalPages(yearsResults, filter.Page, filter.PageSize);
                var paginatedYearsResultsItems = paginatedYearsResults.Items;
                var totalPages = paginatedYearsResults.TotalPages;
                var yearsResultsDto = new List<YearResultDTO>();

                foreach (var yearResult in paginatedYearsResultsItems)
                {
                    var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);
                    var yearResultImage = await _imageService.GetImageById(yearResult.ImageId);

                    yearResultDto.Image = yearResultImage;
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
                throw new ApplicationException("Error retrieving list of years results.", exception);
            }
        }

        public async Task<YearResultDTO> GetYearResultById(int yearResultId, string cultureCode)
        {
            try
            {
                var yearResult = await _yearResultRepository.GetYearResult(yearResultId);
                var yearResultDto = _mapper.Map<YearResultDTO>(yearResult);
                var yearResultImage = await _imageService.GetImageById(yearResult.ImageId);

                yearResultDto.Description = _localizationService.GetLocalizedValue(yearResult.Description, cultureCode, yearResult.DescriptionId);
                yearResultDto.Image = yearResultImage;

                return yearResultDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving year result by id.", exception);
            }
        }
    }
}
