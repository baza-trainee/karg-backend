using AutoMapper;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.Extensions.Configuration;

namespace karg.BLL.Services.Entities
{
    public class ImageService : IImageService
    {
        private readonly IFileService _fileService;
        private readonly IImageRepository _imageRepository;
        private IMapper _mapper;
        private readonly string _baseImagePath;

        public ImageService(IFileService fileService, IImageRepository imageRepository, IMapper mapper, IConfiguration configuration)
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

        public async Task<List<string>> SaveImages(string entityType, int entityId, List<string> imageBytesList, bool isUpdate = false)
        {
            try
            {
                if (isUpdate)
                {
                    await DeleteImages(entityType, entityId);
                }

                var savedImages = new List<string>();
                foreach (var imageBytes in imageBytesList.Select(Convert.FromBase64String))
                {
                    var fileName = Guid.NewGuid() + ".jpg";
                    var folderPath = _fileService.CreateDirectory(Path.Combine(_baseImagePath, entityType.ToLower(), entityId.ToString()));
                    var imageUri = await _fileService.SaveFileAsync(folderPath, imageBytes, fileName);
                    var newImage = new Image { Uri = imageUri };

                    SetImageEntity(newImage, entityType, entityId);
                    await _imageRepository.Add(newImage);
                    savedImages.Add(imageUri);
                }

                return savedImages;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error processing images: {exception.Message}");
            }
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
    }
}