using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class SupplierSvc  : ISupplierSvc
    {
        private readonly LifePetDBContext _context;
        public SupplierSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> AddSupplierAsync(SupplierDTO model)
        {
            var newSupplier = new Supplier
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();
            return newSupplier;

        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier model)
        {
            var supplier = await _context.Suppliers.FindAsync(model.Id);
            if (supplier == null)
            {
                return null;
            }

            // Aggiungere cose (es. supplier.Name = model.Name)

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
