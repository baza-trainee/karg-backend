using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.DAL.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(KargDbContext context) : base(context) { }

        public async Task DeleteRange(IEnumerable<Image> images)
        {
            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}
