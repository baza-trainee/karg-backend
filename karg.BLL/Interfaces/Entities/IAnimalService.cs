using karg.BLL.DTO.Animals;
using karg.BLL.DTO.Utilities;
using Microsoft.AspNetCore.JsonPatch;

namespace karg.BLL.Interfaces.Entities
{
    public interface IAnimalService
    {
        Task<PaginatedResult<AnimalDTO>> GetAnimals(AnimalsFilterDTO filter, string cultureCode);
        Task<CreateAndUpdateAnimalDTO> CreateAnimal(CreateAndUpdateAnimalDTO animalDto);
        Task<CreateAndUpdateAnimalDTO> UpdateAnimal(int animalId, JsonPatchDocument<CreateAndUpdateAnimalDTO> patchDoc);
        Task<AnimalDTO> GetAnimalById(int animalId, string cultureCode);
        Task DeleteAnimal(int animalId);
    }
}
