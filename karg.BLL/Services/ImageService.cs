using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _repository;

        public ImageService(IImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<AllImagesDTO> GetRescuerImage(int imageId)
        {
            try
            {
                var allImages = await _repository.GetImages();
                var rescuerImage = allImages.FirstOrDefault(image => image.Id == imageId);

                return new AllImagesDTO { Uri = rescuerImage.Uri };
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving image of rescuer.", exception);
            }
        }
    }
}
