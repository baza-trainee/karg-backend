using karg.BLL.DTO;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IPartnerMappingService
    {
        Task<List<AllPartnersDTO>> MapToAllPartnersDTO(List<Partner> partners);
    }
}
