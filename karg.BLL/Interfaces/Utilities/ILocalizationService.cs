using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Utilities
{
    public interface ILocalizationService
    {
        string GetLocalizedValue(LocalizationSet localizationSet, string cultureCode, int localizationId);
    }
}
