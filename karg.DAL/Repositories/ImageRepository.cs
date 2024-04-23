using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly KargDbContext _context;

        public ImageRepository(KargDbContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetImages()
        {
            return await _context.Images.AsNoTracking().ToListAsync();
        }
    }
}
