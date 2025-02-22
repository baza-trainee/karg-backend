using AutoMapper;
using karg.BLL.DTO.Advices;
using karg.BLL.DTO.Animals;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Models.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Services.Entities
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IPaginationService<Animal> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public AnimalService(
            IAnimalRepository animalRepository,
            IPaginationService<Animal> paginationService,
            IImageService imageService,
            ILocalizationService localizationService,
            ILocalizationSetService localizationSetService,
            IMapper mapper)
        {
            _animalRepository = animalRepository;
            _localizationSetService = localizationSetService;
            _paginationService = paginationService;
            _imageService = imageService;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public async Task<PaginatedAllAnimalsDTO> GetAnimals(AnimalsFilterDTO filter, string cultureCode)
        {
            try
            {
                var animals = await _animalRepository.GetAll(filter.CategoryFilter, filter.NameSearch);
                var paginatedAnimals = await _paginationService.PaginateWithTotalPages(animals, filter.Page, filter.PageSize);
                var animalsDto = new List<AnimalDTO>();

                foreach (var animal in paginatedAnimals.Items)
                {
                    var animalDto = _mapper.Map<AnimalDTO>(animal);

                    animalDto.Images = (await _imageService.GetImagesByEntity("Animal", animal.Id)).Select(image => image.Uri).ToList();
                    animalDto.Image = animalDto.Images.FirstOrDefault();
                    animalDto.Name = _localizationService.GetLocalizedValue(animal.Name, cultureCode, animal.NameId);
                    animalDto.Story = _localizationService.GetLocalizedValue(animal.Story, cultureCode, animal.StoryId);
                    animalDto.Description = _localizationService.GetLocalizedValue(animal.Description, cultureCode, animal.DescriptionId);
                    animalsDto.Add(animalDto);
                }

                return new PaginatedAllAnimalsDTO
                {
                    Animals = animalsDto,
                    TotalPages = paginatedAnimals.TotalPages
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving list of animals: {exception.Message}");
            }
        }

        public async Task<AnimalDTO> GetAnimalById(int animalId, string cultureCode)
        {
            try
            {
                var animal = await _animalRepository.GetById(animalId);
                if (animal == null) return null;

                var animalDto = _mapper.Map<AnimalDTO>(animal);

                animalDto.Images = (await _imageService.GetImagesByEntity("Animal", animal.Id)).Select(image => image.Uri).ToList();
                animalDto.Image = animalDto.Images.FirstOrDefault();
                animalDto.Name = _localizationService.GetLocalizedValue(animal.Name, cultureCode, animal.NameId);
                animalDto.Story = _localizationService.GetLocalizedValue(animal.Story, cultureCode, animal.StoryId);
                animalDto.Description = _localizationService.GetLocalizedValue(animal.Description, cultureCode, animal.DescriptionId);

                return animalDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error retrieving animal by id: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateAnimalDTO> CreateAnimal(CreateAndUpdateAnimalDTO animalDto)
        {
            try
            {
                var animal = _mapper.Map<Animal>(animalDto);
                animal.NameId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Name_en, animalDto.Name_ua);
                animal.DescriptionId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Description_en, animalDto.Description_ua);
                animal.StoryId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Story_en, animalDto.Story_ua);

                var animalId = await _animalRepository.Add(animal);
                animalDto.Images = await _imageService.UploadImages(nameof(Animal), animalId, animalDto.Images, false);

                return animalDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error adding the animal: {exception.Message}");
            }
        }

        public async Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int animalId, JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc)
        {
            try
            {
                var existingAnimal = await _animalRepository.GetById(animalId);
                var patchedAnimal = _mapper.Map<CreateAndUpdateAnimalDTO>(existingAnimal);

                patchedAnimal.Name_ua = _localizationService.GetLocalizedValue(existingAnimal.Name, "ua", existingAnimal.NameId);
                patchedAnimal.Name_en = _localizationService.GetLocalizedValue(existingAnimal.Name, "en", existingAnimal.NameId);
                patchedAnimal.Description_ua = _localizationService.GetLocalizedValue(existingAnimal.Description, "ua", existingAnimal.DescriptionId);
                patchedAnimal.Description_en = _localizationService.GetLocalizedValue(existingAnimal.Description, "en", existingAnimal.DescriptionId);
                patchedAnimal.Story_ua = _localizationService.GetLocalizedValue(existingAnimal.Story, "ua", existingAnimal.StoryId);
                patchedAnimal.Story_en = _localizationService.GetLocalizedValue(existingAnimal.Story, "en", existingAnimal.StoryId);

                patchDoc.ApplyTo(patchedAnimal);

                if (patchDoc.Operations.Any(op => op.path == "/images"))
                {
                    patchedAnimal.Images = await _imageService.UploadImages(nameof(Animal), animalId, patchedAnimal.Images, true);
                }

                existingAnimal.NameId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.NameId, patchedAnimal.Name_en, patchedAnimal.Name_ua);
                existingAnimal.DescriptionId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.DescriptionId, patchedAnimal.Description_en, patchedAnimal.Description_ua);
                existingAnimal.StoryId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.StoryId, patchedAnimal.Story_en, patchedAnimal.Story_ua);
                existingAnimal.Category = Enum.Parse<AnimalCategory>(patchedAnimal.Category);

                await _animalRepository.Update(existingAnimal);

                return patchedAnimal;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error updating the animal: {exception.Message}");
            }
        }

        public async Task DeleteAnimal(int animalId)
        {
            try
            {
                var removedAnimal = await _animalRepository.GetById(animalId);

                await _imageService.DeleteImages("Animal", removedAnimal.Id);
                await _animalRepository.Delete(removedAnimal);
                await _localizationSetService.DeleteLocalizationSets(new List<int>
                {
                    removedAnimal.NameId,
                    removedAnimal.DescriptionId,
                    removedAnimal.StoryId
                });
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error delete the animal: {exception.Message}");
            }
        }
    }
}