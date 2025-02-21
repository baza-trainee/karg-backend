using karg.DAL.Models;

namespace karg.BLL.Interfaces.Entities
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntity(string entityType, int entityId);
        Task<List<string>> SaveImages(string entityType, int entityId, List<string> imageBytesList, bool isUpdate = false);
        Task DeleteImages(string entityType, int entityId);
    }
}
