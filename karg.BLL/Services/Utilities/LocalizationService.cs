using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Utilities
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
