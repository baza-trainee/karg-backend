using karg.BLL.DTO.Animals;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IAnimalService
    {
        Task<PaginatedAllAnimalsDTO> GetAnimals(AnimalsFilterDTO filter, string cultureCode);
        Task CreateAnimal(CreateAndUpdateAnimalDTO animalDto);
        Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int animalId, JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc);
        Task<AnimalDTO> GetAnimalById(int animalId, string cultureCode);
        Task DeleteAnimal(int animalId);
    }
}
