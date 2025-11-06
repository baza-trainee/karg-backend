using karg.DAL.Models;

namespace karg.BLL.Interfaces.Entities
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntity(string entityType, int entityId);
        Task<List<string>> UploadImages(string entityType, int entityId, List<string> imagesData, bool isUpdate = false);
        Task DeleteImages(string entityType, int entityId);
    }
}
