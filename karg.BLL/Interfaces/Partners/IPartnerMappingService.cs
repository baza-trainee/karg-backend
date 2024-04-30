using karg.BLL.DTO.Partners;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Partners
{
    public interface IPartnerMappingService
    {
        Task<List<AllPartnersDTO>> MapToAllPartnersDTO(List<Partner> partners);
    }
}
