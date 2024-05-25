﻿using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.Interfaces.Advices;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Services.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Advices
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceRepository _adviceRepository;
        private readonly IPaginationService<Advice> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IMapper _mapper;
        public AdviceService(IAdviceRepository adviceRepository, IPaginationService<Advice> paginationService, ILocalizationSetService localizationSetService, IImageService imageService, ILocalizationService localizationService, IMapper mapper)
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
                var advices = await _adviceRepository.GetAdvices();
                var paginatedAdvices = await _paginationService.PaginateWithTotalPages(advices, filter.Page, filter.PageSize);
                var paginatedAdviceItems = paginatedAdvices.Items;
                var totalPages = paginatedAdvices.TotalPages;
                var advicesDto = new List<AdviceDTO>();

                foreach (var advice in paginatedAdviceItems)
                {
                    var adviceDto = _mapper.Map<AdviceDTO>(advice);
                    var adviceImage = await _imageService.GetImageById(advice.ImageId);

                    adviceDto.Image = adviceImage;
                    adviceDto.Description = _localizationService.GetLocalizedValue(advice.Description, cultureCode, advice.DescriptionId);
                    adviceDto.Title = _localizationService.GetLocalizedValue(advice.Title, cultureCode, advice.TitleId);
                    advicesDto.Add(adviceDto);
                }

                return new PaginatedAllAdvicesDTO
                {
                    Advices = advicesDto,
                    TotalPages = totalPages
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of advices.", exception);
            }
        }

        public async Task DeleteAdvice(int id)
        {
            try
            {
                var removedAdvice = await _adviceRepository.GetAdvice(id);
                var removedAdviceTitleId = removedAdvice.TitleId;
                var removedAdviceDescriptionId = removedAdvice.DescriptionId;
                
                await _adviceRepository.DeleteAdvice(removedAdvice);
                await _imageService.DeleteImage(removedAdvice.ImageId);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedAdviceTitleId, removedAdviceDescriptionId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error delete the advice.", exception);
            }
        }
    }
}
