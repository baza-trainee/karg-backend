﻿using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface ILocalizationRepository
    {
        Task<List<Localization>> GetLocalization(int localizationSetId);
        Task UpdateLocalization(Localization updatedLocalisation);
        Task DeleteLocalization(Localization localization);
    }
}
