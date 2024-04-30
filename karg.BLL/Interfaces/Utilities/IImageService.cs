using karg.BLL.DTO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Utilities
{
    public interface IImageService
    {
        Task<List<AllImagesDTO>> GetAnimalImages(int animalId);
        Task<AllImagesDTO> GetImageById(int imageId);
    }
}
