using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using karg.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IPaginationService<Animal> _paginationService;
        private readonly IAnimalMappingService _animalMappingService;

        public AnimalService(IAnimalRepository animalRepository, IPaginationService<Animal> paginationService, IAnimalMappingService animalMappingService)
        {
            _animalRepository = animalRepository;
            _paginationService = paginationService;
            _animalMappingService = animalMappingService;
        }

        public async Task<PaginatedAnimalsDTO> GetAnimals(AnimalsFilterDTO filter)
        {
            try
            {
                var animals = await _animalRepository.GetAnimals(filter.CategoryFilter, filter.NameSearch);
                var paginatedAnimals = await _paginationService.PaginateWithTotalPages(animals, filter.Page, filter.PageSize);
                var paginatedAnimalItems = paginatedAnimals.Items;
                var totalPages = paginatedAnimals.TotalPages;
                var animalsDto = await _animalMappingService.MapToAllAnimalsDTO(paginatedAnimalItems);

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
    }
}
