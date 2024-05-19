using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Utilities
{
    public class LocalizationSetService : ILocalizationSetService
    {
        private readonly ILocalizationSetRepository _localizationSetRepository;
        private readonly ILocalizationRepository _localizationRepository;
        
        public LocalizationSetService(ILocalizationSetRepository localizationSetRepository, ILocalizationRepository localizationRepository)
        {
            _localizationSetRepository = localizationSetRepository;
            _localizationRepository = localizationRepository;
        }

        public async Task<int> CreateAndSaveLocalizationSet(string valueEn, string valueUa)
        {
            try
            {
                var localizationSet = new LocalizationSet
                {
                    Localizations = new List<Localization>
                {
                    new Localization { CultureCode = "en", Value = valueEn },
                    new Localization { CultureCode = "ua", Value = valueUa }
                }
                };

                return await _localizationSetRepository.AddLocalizationSet(localizationSet);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error adding localization set.", exception);
            }
        }

        public async Task DeleteLocalizationSets(List<int> localizationSetIds)
        {
            try
            {
                var localizationSets = await _localizationSetRepository.GetLocalizationSets(localizationSetIds);
                await _localizationSetRepository.DeleteLocalizationSets(localizationSets);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error deleting localization set.", exception);
            }
        }

        public async Task<int> UpdateLocalizationSet(int localizationSetId, string value_en, string value_ua)
        {
            try
            {
                var localizations = await _localizationRepository.GetLocalization(localizationSetId);
                var localization_en = localizations.First(localization => localization.CultureCode == "en");
                var localization_ua = localizations.First(localization => localization.CultureCode == "ua");

                localization_en.Value = value_en;
                localization_ua.Value = value_ua;

                await _localizationRepository.UpdateLocalization(localization_en);
                await _localizationRepository.UpdateLocalization(localization_ua);

                return localizationSetId;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error updating localization set.", exception);
            }
        }
    }
}
