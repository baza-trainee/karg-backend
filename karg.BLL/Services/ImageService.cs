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

        public async Task<List<AllImagesDTO>> GetAnimalImages(int animalId)
        {
            try
            {
                var allImages = await _repository.GetImages();
                var animalsImages = allImages
                    .Where(image => image.AnimalId == animalId)
                    .Select(image => new AllImagesDTO
                    {
                        Uri = image.Uri
                    })
                    .ToList();

                return animalsImages;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving images of animals.", exception);
            }
        }
    }
}
