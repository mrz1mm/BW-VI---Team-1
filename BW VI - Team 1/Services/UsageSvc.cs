using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class UsageSvc
    {
        private readonly LifePetDBContext _context;
        public UsageSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Usage>> GetAllUsagesAsync()
        {
            return await _context.Usages.ToListAsync();
        }

        public async Task<Usage> GetUsageByIdAsync(int id)
        {
            return await _context.Usages.FindAsync(id);
        }

        public async Task<Usage> AddUsageAsync(UsageDTO model)
        {
            var newUsage = new Usage
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Usages.Add(newUsage);
            await _context.SaveChangesAsync();
            return newUsage;

        }

        public async Task<Usage> UpdateUsageAsync(Usage model)
        {
            var animal = await _context.Usages.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            // Aggiungere cose (es. animal.Name = model.Name)

            _context.Usages.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteUsageAsync(int id)
        {
            var animal = await _context.Usages.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Usages.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
