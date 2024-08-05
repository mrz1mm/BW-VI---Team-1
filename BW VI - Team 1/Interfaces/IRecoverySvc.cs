using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IRecoverySvc
    {
        Task<List<Recovery>> GetAllRecoveriesAsync();
        Task<Recovery> GetRecoveryByIdAsync(int id);
        Task<Recovery> GetRecoveryByAnimal(int id);
        Task<Recovery> AddRecoveryAsync(RecoveryDTO recovery);
        Task<Recovery> UpdateRecoveryAsync(Recovery recovery);
        Task<bool> DeleteRecoveryAsync(int id);

    }
}
