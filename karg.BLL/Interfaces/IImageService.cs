using karg.BLL.DTO.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntity(string entityType, int entityId);
        Task AddImages(List<CreateImageDTO> imageDtos);
        Task UpdateEntityImages(string entityType, int entityId, List<string> updatedImagesData);
        Task DeleteImages(string entityType, int entityId);
    }
}
