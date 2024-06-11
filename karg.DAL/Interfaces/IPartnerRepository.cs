using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IPartnerRepository
    {
        Task<List<Partner>> GetPartners();
        Task<Partner> GetPartner(int partnerId);
    }
}
