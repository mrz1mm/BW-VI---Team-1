using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface ILockerSvc
    {
        Task<List<Locker>> GetAllLockersAsync();
        Task<Locker> GetLockerByIdAsync(int id);
        Task<Locker> AddLockerAsync(LockerDTO locker);
        Task<Locker> UpdateLockerAsync(Locker locker);
        Task<bool> DeleteLockerAsync(int id);
    }
}
