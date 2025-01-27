using karg.BLL.DTO.Utilities;
using karg.DAL.Models;

namespace karg.BLL.Interfaces.Entities
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntity(string entityType, int entityId);
        Task AddImages(List<CreateImageDTO> imageDtos);
        Task UpdateEntityImages(string entityType, int entityId, List<Uri> updatedImageUris);
        Task DeleteImages(string entityType, int entityId);
    }
}
