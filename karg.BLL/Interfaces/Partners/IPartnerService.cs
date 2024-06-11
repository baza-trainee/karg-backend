using karg.BLL.DTO.Partners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Partners
{
    public interface IPartnerService
    {
        Task<List<AllPartnersDTO>> GetPartners();
        Task DeletePartner(int partnerId);
    }
}
