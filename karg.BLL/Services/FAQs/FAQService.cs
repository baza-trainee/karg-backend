﻿using AutoMapper;
using karg.BLL.DTO.FAQs;
using karg.BLL.Interfaces.FAQs;
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
        private readonly IMapper _mapper;

        public FAQService(IFAQRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }

        public async Task<List<AllFAQsDTO>> GetFAQs()
        {
            try
            {
                var faqs = await _faqRepository.GetFAQs();
                var faqsDto = _mapper.Map<List<AllFAQsDTO>>(faqs);

                return faqsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of FAQs.", exception);
            }
        }
    }
}
