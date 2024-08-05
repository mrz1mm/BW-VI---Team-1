using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class OwnerSvc : IOwnerSvc
    {
        private readonly LifePetDBContext _context;
        public OwnerSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Owner>> GetAllOwnersAsync()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<Owner> GetOwnerByIdAsync(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public async Task<Owner> AddOwnerAsync(OwnerDTO model)
        {
            var newOwner = new Owner
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                FiscalCode = model.FiscalCode
            };
            _context.Owners.Add(newOwner);
            await _context.SaveChangesAsync();
            return newOwner;
        }

        public async Task<Owner> UpdateOwnerAsync(Owner model)
        {
            var owner = await _context.Owners.FindAsync(model.Id);
            if (owner == null)
            {
                return null;
            }

            owner.FirstName = model.FirstName;
            owner.LastName = model.LastName;
            owner.FiscalCode = model.FiscalCode;

            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return owner;
        }

        public async Task DeleteOwnerAsync(int id)
        {
            var ownerToDelete = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);
            if (ownerToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Owners.Remove(ownerToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
