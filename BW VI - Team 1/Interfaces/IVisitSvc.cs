using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IVisitSvc
    {
        Task<List<Visit>> GetAllVisitsAsync();
        Task<Visit> GetVisitByIdAsync(int id);
        Task<Visit> AddVisitAsync(VisitDTO visit);
        Task<Visit> UpdateVisitAsync(Visit visit);
        Task<bool> DeleteVisitAsync(int id);
    }
}
