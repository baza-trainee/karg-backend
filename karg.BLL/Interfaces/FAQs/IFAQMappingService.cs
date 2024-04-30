using karg.BLL.DTO.FAQs;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.FAQs
{
    public interface IFAQMappingService
    {
        Task<List<AllFAQsDTO>> MapToAllFAQsDTO(List<FAQ> faqs);
    }
}
