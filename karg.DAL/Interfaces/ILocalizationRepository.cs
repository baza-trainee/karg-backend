using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface ILocalizationRepository : IBaseRepository<Localization> 
    {
        Task<List<Localization>> GetLocalizationBySetId(int localizationSetId);
    }
}