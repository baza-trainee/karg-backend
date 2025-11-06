using karg.DAL.Models;

namespace karg.BLL.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string GetLocalizedValue(LocalizationSet localizationSet, string cultureCode, int localizationId);
    }
}
