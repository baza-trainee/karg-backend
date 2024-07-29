using karg.BLL.Interfaces;
using karg.DAL.Interfaces;

namespace karg.BLL.Services
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
            var cultures = await _cultureRepository.GetAll();
            var culturesCodes = cultures.Select(culture => culture.Code);

            return culturesCodes.Contains(cultureCode);
        }
    }
}
