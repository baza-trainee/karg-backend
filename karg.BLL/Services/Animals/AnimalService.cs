using AutoMapper;
using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Animals;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace karg.BLL.Services.Animals
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly ILocalizationSetService _localizationSetService;
        private readonly IPaginationService<Animal> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public AnimalService(IAnimalRepository animalRepository, IPaginationService<Animal> paginationService, IImageService imageService,ILocalizationService localizationService, ILocalizationSetService localizationSetService, IMapper mapper)
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
                var animals = await _animalRepository.GetAnimals(filter.CategoryFilter, filter.NameSearch);
                var paginatedAnimals = await _paginationService.PaginateWithTotalPages(animals, filter.Page, filter.PageSize);
                var paginatedAnimalItems = paginatedAnimals.Items;
                var totalPages = paginatedAnimals.TotalPages;
                var animalsDto = new List<AnimalDTO>();

                foreach (var animal in paginatedAnimalItems)
                {
                    var animalDto = _mapper.Map<AnimalDTO>(animal);
                    var animalImages = await _imageService.GetAnimalImages(animal.Id);

                    animalDto.Name = _localizationService.GetLocalizedValue(animal.Name, cultureCode, animal.NameId);
                    animalDto.Story = _localizationService.GetLocalizedValue(animal.Story, cultureCode, animal.StoryId);
                    animalDto.Description = _localizationService.GetLocalizedValue(animal.Description, cultureCode, animal.DescriptionId);
                    animalDto.Images = animalImages.Select(image => image.Uri).ToList();
                    animalDto.Image = animalDto.Images.FirstOrDefault();
                    animalsDto.Add(animalDto);
                }

                return new PaginatedAllAnimalsDTO
                {
                    Animals = animalsDto,
                    TotalPages = totalPages
                };
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of animals.", exception);
            }
        }

        public async Task<AnimalDTO> GetAnimalById(int animalId, string cultureCode)
        {
            try
            {
                var animal = await _animalRepository.GetAnimal(animalId);
                var animalDto = _mapper.Map<AnimalDTO>(animal);
                var animalImages = await _imageService.GetAnimalImages(animal.Id);

                animalDto.Name = _localizationService.GetLocalizedValue(animal.Name, cultureCode, animal.NameId);
                animalDto.Story = _localizationService.GetLocalizedValue(animal.Story, cultureCode, animal.StoryId);
                animalDto.Description = _localizationService.GetLocalizedValue(animal.Description, cultureCode, animal.DescriptionId);
                animalDto.Images = animalImages.Select(image => image.Uri).ToList();
                animalDto.Image = animalDto.Images.FirstOrDefault();

                return animalDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving animal by id.", exception);
            }
        }

        public async Task CreateAnimal(CreateAndUpdateAnimalDTO animalDto)
        {
            try
            {
                var animal = _mapper.Map<Animal>(animalDto);
                animal.NameId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Name_en, animalDto.Name_ua);
                animal.DescriptionId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Description_en, animalDto.Description_ua);
                animal.StoryId = await _localizationSetService.CreateAndSaveLocalizationSet(animalDto.Story_en, animalDto.Story_ua);

                var animalId = await _animalRepository.AddAnimal(animal);

                foreach (var image in animalDto.Images)
                {
                    var newImage = new CreateImageDTO
                    {
                        Uri = image,
                        AnimalId = animalId,
                    };

                    await _imageService.AddImage(newImage);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error adding the animal.", exception);
            }
        }

        public async Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int animalId, JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc)
        {
            try
            {
                var existingAnimal = await _animalRepository.GetAnimal(animalId);
                var patchedAnimal = _mapper.Map<CreateAndUpdateAnimalDTO>(existingAnimal);

                patchedAnimal.Name_ua = _localizationService.GetLocalizedValue(existingAnimal.Name, "ua", existingAnimal.NameId);
                patchedAnimal.Name_en = _localizationService.GetLocalizedValue(existingAnimal.Name, "en", existingAnimal.NameId);
                patchedAnimal.Description_ua = _localizationService.GetLocalizedValue(existingAnimal.Description, "ua", existingAnimal.DescriptionId);
                patchedAnimal.Description_en = _localizationService.GetLocalizedValue(existingAnimal.Description, "en", existingAnimal.DescriptionId);
                patchedAnimal.Story_ua = _localizationService.GetLocalizedValue(existingAnimal.Story, "ua", existingAnimal.StoryId);
                patchedAnimal.Story_en = _localizationService.GetLocalizedValue(existingAnimal.Story, "en", existingAnimal.StoryId);

                var animalImages = await _imageService.GetAnimalImages(animalId);
                patchedAnimal.Images = animalImages.Select(image => image.Uri).ToList();

                patchDoc.ApplyTo(patchedAnimal);

                await _imageService.UpdateAnimalImages(animalId, patchedAnimal.Images);

                existingAnimal.NameId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.NameId, patchedAnimal.Name_en, patchedAnimal.Name_ua);
                existingAnimal.DescriptionId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.DescriptionId, patchedAnimal.Description_en, patchedAnimal.Description_ua);
                existingAnimal.StoryId = await _localizationSetService.UpdateLocalizationSet(existingAnimal.StoryId, patchedAnimal.Story_en, patchedAnimal.Story_ua);

                await _animalRepository.UpdateAnimal(existingAnimal);

                return patchedAnimal;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error updating the animal.", exception);
            }
        }

        public async Task DeleteAnimal(int animalId)
        {
            try
            {
                var removedAnimal = await _animalRepository.GetAnimal(animalId);
                var removedAnimalNameId = removedAnimal.NameId;
                var removedAnimalDescriptionId = removedAnimal.DescriptionId;
                var removedAnimalStoryId = removedAnimal.StoryId;

                await _animalRepository.DeleteAnimal(removedAnimal);
                await _localizationSetService.DeleteLocalizationSets(new List<int> { removedAnimalNameId, removedAnimalDescriptionId, removedAnimalStoryId });
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error delete the animal.", exception);
            }
        }
    }
}