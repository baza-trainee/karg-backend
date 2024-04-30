using karg.BLL.DTO.Animals;
using karg.BLL.Interfaces.Animals;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Animals
{
    public class AnimalMappingService : IAnimalMappingService
    {
        private readonly IImageService _imageService;

        public AnimalMappingService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<List<AllAnimalsDTO>> MapToAllAnimalsDTO(List<Animal> animals)
        {
            try
            {
                var animalsDTO = new List<AllAnimalsDTO>();
                foreach (var animal in animals)
                {
                    var animalImages = await _imageService.GetAnimalImages(animal.Id);
                    animalsDTO.Add(new AllAnimalsDTO
                    {
                        Name = animal.Name,
                        Category = animal.Category.ToString(),
                        Description = animal.Description,
                        Story = animal.Story,
                        Image = animalImages.FirstOrDefault().Uri,
                        Images = animalImages.Select(image => image.Uri).ToList()
                    });
                }

                return animalsDTO;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("An error occurred while mapping animals to DTOs:", exception);
            }
        }
    }
}
