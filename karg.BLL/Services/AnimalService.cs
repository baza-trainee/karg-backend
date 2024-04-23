using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
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
        private readonly IImageService _imageService;

        public AnimalService(IAnimalRepository animalRepository, IImageService imageService)
        {
            _animalRepository = animalRepository;
            _imageService = imageService;
        }

        public async Task<List<AllAnimalsDTO>> GetAnimals(AnimalsFilterDTO filter)
        {
            try
            {
                var animals = await _animalRepository.GetAnimals(filter.Page, filter.PageSize, filter.CategoryFilter, filter.NameSearch);
                var animalsDto = new List<AllAnimalsDTO>();

                foreach (var animal in animals)
                {
                    var animalImages = await _imageService.GetAnimalImages(animal.Id);

                    var animalDto = new AllAnimalsDTO
                    {
                        Name = animal.Name,
                        Category = animal.Category.ToString(),
                        Description = animal.Description,
                        Story = animal.Story,
                        Image = animalImages.FirstOrDefault().Uri
                    };

                    animalsDto.Add(animalDto);
                }

                return animalsDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of animals.", exception);
            }
        }
    }
}
