using AutoMapper;
using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Animals;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Animals
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IPaginationService<Animal> _paginationService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public AnimalService(IAnimalRepository animalRepository, IPaginationService<Animal> paginationService, IImageService imageService, IMapper mapper)
        {
            _animalRepository = animalRepository;
            _paginationService = paginationService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<PaginatedAnimalsDTO> GetAnimals(AnimalsFilterDTO filter)
        {
            try
            {
                var animals = await _animalRepository.GetAnimals(filter.CategoryFilter, filter.NameSearch);
                var paginatedAnimals = await _paginationService.PaginateWithTotalPages(animals, filter.Page, filter.PageSize);
                var paginatedAnimalItems = paginatedAnimals.Items;
                var totalPages = paginatedAnimals.TotalPages;
                var animalsDto = new List<AnimalDTO>();

                foreach(var animal in paginatedAnimalItems)
                {
                    var animalDto = _mapper.Map<AnimalDTO>(animal);
                    var animalImages = await _imageService.GetAnimalImages(animal.Id);

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

        public async Task<AnimalDTO> GetAnimalById(int id)
        {
            try
            {
                var animal = await _animalRepository.GetAnimal(id);
                var animalDto = _mapper.Map<AnimalDTO>(animal);
                var animalImages = await _imageService.GetAnimalImages(animal.Id);

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

        public async Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int id, CreateAndUpdateAnimalDTO animalDto)
        {
            try
            {
                var updatedAnimal = _mapper.Map<Animal>(animalDto);
                var existingAnimal = await _animalRepository.GetAnimal(id);

                await _imageService.UpdateAnimalImages(id, animalDto.Images);
                await _animalRepository.UpdateAnimal(existingAnimal, updatedAnimal);

                return animalDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error update the animal.", exception);
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
