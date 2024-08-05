using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;
using System.Linq;

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
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
            };
            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();
            return newSupplier;

        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier model)
        {
            var UpdatedSupplier = await _context.Suppliers.FindAsync(model.Id);
            if (UpdatedSupplier == null)
            {
                return null;
            }

            UpdatedSupplier.Name = model.Name;
            UpdatedSupplier.Address = model.Address;
            UpdatedSupplier.Telephone = model.Telephone;
            var productIds = model.Products.Select(p => p.Id).ToList();

            UpdatedSupplier.Products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            _context.Suppliers.Update(UpdatedSupplier);
            await _context.SaveChangesAsync();
            return UpdatedSupplier;
        }


        public async Task DeleteSupplierAsync(int id)
        {
            var SupplierDelete = await _context.Suppliers.FirstOrDefaultAsync(o => o.Id == id);
            if (SupplierDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Suppliers.Remove(SupplierDelete);
            await _context.SaveChangesAsync();
        }
    }
}
