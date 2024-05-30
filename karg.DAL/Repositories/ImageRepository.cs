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

        public async Task<Image> GetImage(int imageId)
        {
            return await _context.Images.AsNoTracking().FirstOrDefaultAsync(image => image.Id == imageId);
        }

        public async Task UpdateImage(Image updatedImage)
        {
            var existingImage = await _context.Images.FindAsync(updatedImage.Id);

            _context.Entry(existingImage).CurrentValues.SetValues(updatedImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(Image image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddImage(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image.Id;
        }
    }
}
