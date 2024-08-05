using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IAnimalSvc
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task<Animal> AddAnimalAsync(AnimalDTO animal);
        Task<Animal> UpdateAnimalAsync(Animal animal);
        Task<bool> DeleteAnimalAsync(int id);
    }
}
