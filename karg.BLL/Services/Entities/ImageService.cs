using AutoMapper;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace karg.BLL.Services.Entities
{
    public class ImageService : IImageService
    {
        private readonly IFileService _fileService;
        private readonly IImageRepository _imageRepository;
        private IMapper _mapper;
        private readonly string _baseImagePath;

        public ImageService(
            IFileService fileService,
            IImageRepository imageRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _fileService = fileService;
            _imageRepository = imageRepository;
            _mapper = mapper;
            _baseImagePath = Path.Combine(configuration["FileStoragePath"], "uploads");
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

        public async Task<List<string>> UploadImages(string entityType, int entityId, List<string> imagesData, bool isUpdate = false)
        {
            var uploadedImageUris = new List<string>();
            var entityFolderPath = _fileService.CreateDirectory(Path.Combine(_baseImagePath, entityType.ToLower(), entityId.ToString()));

            if (isUpdate)
            {
                await RemoveObsoleteImages(entityType, entityId, imagesData);
            }

            foreach (var imageData in imagesData)
            {
                var imageUri = await StoreImage(imageData, entityFolderPath, entityType, entityId);
                uploadedImageUris.Add(imageUri);
            }

            return uploadedImageUris; 
        }

        public async Task DeleteImages(string entityType, int entityId)
        {
            try
            {
                var imagesToRemove = await GetImagesByEntity(entityType, entityId);
                string folderPath = Path.Combine(_baseImagePath, entityType.ToLower(), entityId.ToString());

                _fileService.DeleteDirectory(folderPath);
                await _imageRepository.DeleteRange(imagesToRemove);
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error deleting the images: {exception.Message}");
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

        private void SetImageEntity(Image imageDto, string entityType, int entityId)
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

        private bool IsUri(string imageData)
        {
            return imageData.StartsWith("/img");
        }

        private async Task RemoveObsoleteImages(string entityType, int entityId, List<string> updatedImageUris)
        {
            var existingImages = await GetImagesByEntity(entityType, entityId);
            var imagesToDelete = existingImages.Where(img => !updatedImageUris.Contains(img.Uri)).ToList();

            foreach (var image in imagesToDelete)
            {
                _fileService.DeleteImageFile(image.Uri);
                await _imageRepository.Delete(image);
            }
        }

        private async Task<string> StoreImage(string imageData, string folderPath, string entityType, int entityId)
        {
            if (IsUri(imageData))
            {
                return imageData;
            }

            var imageBytes = Convert.FromBase64String(imageData);
            var fileName = $"{Guid.NewGuid()}.jpg";
            var imageUri = await _fileService.SaveImageFile(folderPath, imageBytes, fileName);

            await RegisterImage(entityType, entityId, imageUri);

            return imageUri;
        }

        private async Task RegisterImage(string entityType, int entityId, string imageUri)
        {
            var newImage = new Image { Uri = imageUri };
            SetImageEntity(newImage, entityType, entityId);
            await _imageRepository.Add(newImage);
        }
    }
}