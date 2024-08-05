using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class OwnerSvc
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
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Owners.Add(newOwner);
            await _context.SaveChangesAsync();
            return newOwner;

        }

        public async Task<Owner> UpdateOwnerAsync(Owner model)
        {
            var animal = await _context.Owners.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            // Aggiungere cose (es. animal.Name = model.Name)

            _context.Owners.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteOwnerAsync(int id)
        {
            var animal = await _context.Owners.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Owners.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
