using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface ILocalizationSetService
    {
        Task<int> CreateAndSaveLocalizationSet(string valueEn, string valueUa);
        Task DeleteLocalizationSets(List<int> localizationSetIds);
        Task<int> UpdateLocalizationSet(int localizationSetId, string value_en, string value_ua);
    }
}
