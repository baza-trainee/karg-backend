using karg.DAL.Models;

namespace karg.BLL.Interfaces.Entities
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntity(string entityType, int entityId);
        Task AddImages(string entityType, int entityId, List<byte[]> imageBytesList);
        Task UpdateEntityImages(string entityType, int entityId, List<byte[]> updatedImages);
        Task DeleteImages(string entityType, int entityId);
    }
}
