using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface ILocalizationSetRepository : IBaseRepository<LocalizationSet>
    {
        Task DeleteLocalizationSets(List<LocalizationSet> localizationSets);
        Task<List<LocalizationSet>> GetLocalizationSets(List<int> localizationSetIds);
    }
}