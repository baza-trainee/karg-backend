using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Utilities
{
    public class CultureService : ICultureService
    {
        private readonly ICultureRepository _cultureRepository;

        public CultureService(ICultureRepository cultureRepository)
        {
            _cultureRepository = cultureRepository;
        }

        public async Task<bool> IsCultureCodeInDatabase(string cultureCode)
        {
            var cultures = await _cultureRepository.GetCultures();
            var culturesCodes = cultures.Select(culture => culture.Code);

            return culturesCodes.Contains(cultureCode);
        }
    }
}
