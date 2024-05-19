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
        private readonly IPaginationService<Animal> _paginationService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public AnimalService(IAnimalRepository animalRepository, IPaginationService<Animal> paginationService, IImageService imageService,ILocalizationService localizationService, IMapper mapper)
        {
            _animalRepository = animalRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public async Task<PaginatedAnimalsDTO> GetAnimals(AnimalsFilterDTO filter, string cultureCode)
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
                    animalDto.Short_Description = _localizationService.GetLocalizedValue(animal.Short_Description, cultureCode, animal.Short_DescriptionId);
                    animalDto.Description = _localizationService.GetLocalizedValue(animal.Description, cultureCode, animal.DescriptionId);
                    animalDto.Images = animalImages.Select(image => image.Uri).ToList();
                    animalDto.Image = animalDto.Images.FirstOrDefault();
                    animalsDto.Add(animalDto);
                }

                return new PaginatedAnimalsDTO
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

        public async Task<AnimalDTO> GetAnimalById(int id, string cultureCode)
        {
            try
            {
                var animal = await _animalRepository.GetAnimal(id);
                var animalDto = _mapper.Map<AnimalDTO>(animal);
                var animalImages = await _imageService.GetAnimalImages(animal.Id);

                animalDto.Name = _localizationService.GetLocalizedValue(animal.Name, cultureCode, animal.NameId);
                animalDto.Story = _localizationService.GetLocalizedValue(animal.Story, cultureCode, animal.StoryId);
                animalDto.Short_Description = _localizationService.GetLocalizedValue(animal.Short_Description, cultureCode, animal.Short_DescriptionId);
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

        public async Task CreateAnimal(CreateAnimalDTO animalDto)
        {
            try
            {
                var animal = _mapper.Map<Animal>(animalDto);
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

        public async Task<AnimalDTO> UpdateAnimal(int id, JsonPatchDocument<AnimalDTO> patchDoc)
        {
            try
            {
                /*var existingAnimal = await GetAnimalById(id);
                patchDoc.ApplyTo(existingAnimal);

                await _imageService.UpdateAnimalImages(id, existingAnimal.Images);

                var mappedAnimal = _mapper.Map<Animal>(existingAnimal);
                await _animalRepository.UpdateAnimal(mappedAnimal);

                return existingAnimal;*/
                return null;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error updating the animal.", exception);
            }
        }

        public async Task DeleteAnimal(int id)
        {
            try
            {
                var removedAnimal = await _animalRepository.GetAnimal(id);
                await _animalRepository.Delete(removedAnimal);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error delete the animal.", exception);
            }
        }
    }
}