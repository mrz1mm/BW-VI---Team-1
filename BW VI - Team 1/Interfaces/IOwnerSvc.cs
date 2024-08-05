using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IOwnerSvc
    {
        Task<List<Owner>> GetAllOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(int id);
        Task<Owner> AddOwnerAsync(OwnerDTO owner);
        Task<Owner> UpdateOwnerAsync(Owner owner);
        Task<bool> DeleteOwnerAsync(int id);

    }
}
