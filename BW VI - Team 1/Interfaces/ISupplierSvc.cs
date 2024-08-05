using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface ISupplierSvc
    {
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<Supplier> AddSupplierAsync(SupplierDTO supplier);
        Task<Supplier> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
