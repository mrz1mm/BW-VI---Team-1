using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class RecoverySvc
    {
        private readonly LifePetDBContext _context;
        public RecoverySvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Recovery>> GetAllRecoverysAsync()
        {
            return await _context.Recoverys.ToListAsync();
        }

        public async Task<Recovery> GetRecoveryByIdAsync(int id)
        {
            return await _context.Recoverys.FindAsync(id);
        }

        public async Task<Recovery> AddRecoveryAsync(RecoveryDTO model)
        {
            var newRecovery = new Recovery
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Recoverys.Add(newRecovery);
            await _context.SaveChangesAsync();
            return newRecovery;

        }

        public async Task<Recovery> UpdateRecoveryAsync(Recovery model)
        {
            var animal = await _context.Recoverys.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            // Aggiungere cose (es. animal.Name = model.Name)

            _context.Recoverys.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteRecoveryAsync(int id)
        {
            var animal = await _context.Recoverys.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Recoverys.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
