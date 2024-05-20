using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface ILocalizationSetRepository
    {
        Task DeleteLocalizationSets(List<LocalizationSet> localizationSets);
        Task<List<LocalizationSet>> GetLocalizationSets(List<int> localizationSetIds);
        Task<int> AddLocalizationSet(LocalizationSet localizationSet);
    }
}
