using AutoMapper;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using karg.DAL.Models;

namespace karg.BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private IMapper _mapper;

        public ImageService(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<List<Image>> GetImagesByEntity(string entityType, int entityId)
        {
            try
            {
                var allImages = await _imageRepository.GetAll();
                var entityImages = allImages.Where(image => IsImageMatchingEntity(image, entityType, entityId)).ToList();
                return entityImages;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving images: {exception.Message}");
            }
        }

        public async Task AddImages(List<CreateImageDTO> imageDtos)
        {
            try
            {
                foreach (var imageDto in imageDtos)
                {
                    var image = _mapper.Map<Image>(imageDto);
                    await ValidateImageEntity(image);
                    await _imageRepository.Add(image);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding images: {exception.Message}");
            }
        }

        public async Task UpdateEntityImages(string entityType, int entityId, List<string> updatedImagesData)
        {
            try
            {
                var existingImages = await GetImagesByEntity(entityType, entityId);

                await _imageRepository.DeleteRange(existingImages);

                foreach (var updatedImageData in updatedImagesData)
                {
                    var newImageDto = new CreateImageDTO { Base64Data = updatedImageData };
                    SetImageEntity(newImageDto, entityType, entityId);
                    await AddImage(newImageDto);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error when updating entity images: {exception.Message}");
            }
        }

        public async Task DeleteImages(string entityType, int entityId)
        {
            try
            {
                var removedImage = await GetImagesByEntity(entityType, entityId);
                await _imageRepository.DeleteRange(removedImage);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error deleting the images: {exception.Message}");
            }
        }

        private async Task<int> AddImage(CreateImageDTO imageDto)
        {
            try
            {
                var image = _mapper.Map<Image>(imageDto);
                await ValidateImageEntity(image);
                await _imageRepository.Add(image);
                return image.Id;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the image: {exception.Message}");
            }
        }

        private async Task ValidateImageEntity(Image image)
        {
            var entityIds = new List<int?> { image.AnimalId, image.AdviceId, image.RescuerId, image.PartnerId, image.YearResultId };
            var associatedEntitiesCount = entityIds.Count(id => id != null);

            if (associatedEntitiesCount == 0)
            {
                throw new ApplicationException("Image must be associated with at least one entity.");
            }

            if (associatedEntitiesCount > 1)
            {
                throw new ApplicationException("Image cannot be associated with more than one entity.");
            }
        }

        private bool IsImageMatchingEntity(Image image, string entityType, int entityId)
        {
            return entityType switch
            {
                nameof(Animal) => image.AnimalId == entityId,
                nameof(Advice) => image.AdviceId == entityId,
                nameof(Rescuer) => image.RescuerId == entityId,
                nameof(Partner) => image.PartnerId == entityId,
                nameof(YearResult) => image.YearResultId == entityId,
                _ => false,
            };
        }

        private void SetImageEntity(CreateImageDTO imageDto, string entityType, int entityId)
        {
            switch (entityType)
            {
                case nameof(Animal):
                    imageDto.AnimalId = entityId;
                    break;
                case nameof(Advice):
                    imageDto.AdviceId = entityId;
                    break;
                case nameof(Rescuer):
                    imageDto.RescuerId = entityId;
                    break;
                case nameof(Partner):
                    imageDto.PartnerId = entityId;
                    break;
                case nameof(YearResult):
                    imageDto.YearResultId = entityId;
                    break;
            }
        }
    }
}