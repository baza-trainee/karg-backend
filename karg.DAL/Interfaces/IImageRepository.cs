﻿using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IImageRepository
    {
        Task<List<Image>> GetImages();
        Task<int> AddImage(Image image);
    }
}
