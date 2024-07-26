using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface IImageRepository : IBaseRepository<Image> 
    {
        Task DeleteRange(IEnumerable<Image> images);
    }
}
