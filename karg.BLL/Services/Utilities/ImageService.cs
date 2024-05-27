using AutoMapper;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Utilities
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _repository;
        private IMapper _mapper;

        public ImageService(IImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddImage(CreateImageDTO imageDto)
        {
            try
            {
                var image = _mapper.Map<Image>(imageDto);

                await _repository.AddImage(image);

                return image.Id;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error adding the image.", exception);
            }
        }

        public async Task<Uri> GetImageById(int imageId)
        {
            try
            {
                var allImages = await _repository.GetImages();
                var image = allImages.FirstOrDefault(image => image.Id == imageId);

                return image.Uri;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving image.", exception);
            }
        }

        public async Task<List<Image>> GetAnimalImages(int animalId)
        {
            try
            {
                var allImages = await _repository.GetImages();
                var animalsImages = allImages
                    .Where(image => image.AnimalId == animalId)
                    .ToList();

                return animalsImages;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving images of animals.", exception);
            }
        }

        public async Task UpdateAnimalImages(int animalId, List<Uri> updatedImageUris)
        {
            try
            {
                var existingImages = await GetAnimalImages(animalId);
                var existingImageUris = existingImages.Select(image => image.Uri).ToList();
                var deletedImages = existingImages
                    .Where(existingImage => !updatedImageUris.Any(updatedImageUri => updatedImageUri == existingImage.Uri))
                    .ToList();
                var newImages = updatedImageUris.Except(existingImageUris).ToList();

                foreach (var image in deletedImages)
                {
                    await _repository.DeleteImage(image);
                }

                foreach (var image in newImages)
                {
                    var newAnimalImage = new CreateImageDTO { AnimalId = animalId, Uri = image };
                    await AddImage(newAnimalImage);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error when updating animal images.", exception);
            }
        }

        public async Task DeleteImage(int imageId)
        {
            try
            {
                var allImages = await _repository.GetImages();
                var image = allImages.FirstOrDefault(image => image.Id == imageId);

                await _repository.DeleteImage(image);
            }
            catch(Exception exception)
            {
                throw new ApplicationException("Error delete the image.", exception);
            }
        }
    }
}
