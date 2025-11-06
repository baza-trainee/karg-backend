using karg.BLL.Interfaces.Localization;
using karg.DAL.Models;

namespace karg.BLL.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public string GetLocalizedValue(LocalizationSet localizationSet, string cultureCode, int localizationId)
        {
            return localizationSet.Localizations
                .FirstOrDefault(localization => string.Equals(localization.CultureCode, cultureCode, StringComparison.OrdinalIgnoreCase) && localization.LocalizationSetId == localizationId)?.Value;
        }
    }
}