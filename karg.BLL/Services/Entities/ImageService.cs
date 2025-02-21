using AutoMapper;
using karg.BLL.Interfaces.Entities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace karg.BLL.Services.Entities
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private IMapper _mapper;
        private readonly string _baseImagePath;

        public ImageService(IImageRepository imageRepository, IMapper mapper, IConfiguration configuration)
        {
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

        public async Task AddImages(string entityType, int entityId, List<byte[]> imageBytesList)
        {
            string entityFolder = CreateEntityFolder(entityType, entityId);

            foreach (var imageBytes in imageBytesList)
            {
                string fileName = Guid.NewGuid() + ".jpg";
                string filePath = Path.Combine(entityFolder, fileName);

                await File.WriteAllBytesAsync(filePath, imageBytes);

                var newImage = new Image
                {
                    Uri = filePath.Replace("\\", "/")
                };
                SetImageEntity(newImage, entityType, entityId);
                await _imageRepository.Add(newImage);
            }
        }

        public async Task UpdateEntityImages(string entityType, int entityId, List<byte[]> updatedImages)
        {
            var existingImages = await GetImagesByEntity(entityType, entityId);
            foreach (var image in existingImages)
            {
                DeleteFile(image.Uri.ToString());
            }
            await _imageRepository.DeleteRange(existingImages);
            await AddImages(entityType, entityId, updatedImages);
        }

        public async Task DeleteImages(string entityType, int entityId)
        {
            var imagesToRemove = await GetImagesByEntity(entityType, entityId);
            string folderPath = Path.Combine(_baseImagePath, entityType.ToLower(), entityId.ToString());
            Directory.Delete(folderPath, true);

            await _imageRepository.DeleteRange(imagesToRemove);
        }

        private string CreateEntityFolder(string entityType, int entityId)
        {
            string folderPath = Path.Combine(_baseImagePath, entityType.ToLower(), entityId.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            
            }
            return folderPath;
        }

        private void DeleteFile(string fileUri)
        {
            string filePath = Path.Combine(_baseImagePath, fileUri.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
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
    }
}