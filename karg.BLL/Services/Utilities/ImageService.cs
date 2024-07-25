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

                await _repository.Add(image);

                return image.Id;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the image: {exception.Message}");
            }
        }

        public async Task<Uri> GetImageById(int imageId)
        {
            try
            {
                var image = await _repository.GetById(imageId);

                return image.Uri;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving image: {exception.Message}");
            }
        }

        public async Task<List<Image>> GetAnimalImages(int animalId)
        {
            try
            {
                var allImages = await _repository.GetAll();
                var animalsImages = allImages
                    .Where(image => image.AnimalId == animalId)
                    .ToList();

                return animalsImages;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving images of animals: {exception.Message}");
            }
        }

        public async Task UpdateImage(int imageId, Uri updatedImageUri)
        {
            try
            {
                var existingImage = await _repository.GetById(imageId);
                if (existingImage.Uri != updatedImageUri)
                {
                    existingImage.Uri = updatedImageUri;

                    await _repository.Update(existingImage);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error when updating image: {exception.Message}");
            }
        }

        public async Task UpdateAnimalImages(int animalId, List<Uri> updatedImagesUris)
        {
            try
            {
                var existingImages = await GetAnimalImages(animalId);

                foreach (var image in existingImages)
                {
                    await _repository.Delete(image);
                }

                foreach (var updatedImageUri in updatedImagesUris)
                {
                    var newAnimalImage = new CreateImageDTO { AnimalId = animalId, Uri = updatedImageUri };
                    await AddImage(newAnimalImage);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error when updating animal images: {exception.Message}");
            }
        }

        public async Task DeleteImage(int imageId)
        {
            try
            {
                var allImages = await _repository.GetAll();
                var image = allImages.FirstOrDefault(image => image.Id == imageId);

                await _repository.Delete(image);
            }
            catch(Exception exception)
            {
                throw new ApplicationException($"Error delete the image: {exception.Message}");
            }
        }
    }
}
