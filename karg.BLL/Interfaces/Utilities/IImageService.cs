using karg.BLL.DTO.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Utilities
{
    public interface IImageService
    {
        Task<List<Image>> GetAnimalImages(int animalId);
        Task<Uri> GetImageById(int imageId);
        Task DeleteImage(int imageId);
        Task AddImage(CreateImageDTO imageDto);
        Task UpdateAnimalImages(int animalId, List<Uri> updatedImageUris);
    }
}
