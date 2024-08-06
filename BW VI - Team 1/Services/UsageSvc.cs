using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class UsageSvc : IUsageSvc
    {
        private readonly LifePetDBContext _context;
        public UsageSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Usage>> GetAllUsagesAsync()
        {
            return await _context.Usages
                .Include(u => u.Products) 
                .ToListAsync();
        }

        public async Task<Usage> GetUsageByIdAsync(int id)
        {
            return await _context.Usages
                .Include(u => u.Products) 
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usage> AddUsageAsync(UsageDTO model)
        {
            var newUsage = new Usage
            {
                Description = model.Description,
            };

            _context.Usages.Add(newUsage);
            await _context.SaveChangesAsync();
            return newUsage;
        }

        public async Task<Usage> UpdateUsageAsync(UsageDTO model, int id)
        {
            var usage = await _context.Usages
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usage == null)
            {
                return null;
            }

            usage.Description = model.Description;

            _context.Usages.Update(usage);
            await _context.SaveChangesAsync();
            return usage;
        }

        public async Task DeleteUsageAsync(int id)
        {
            var usageDelete = await _context.Usages.FindAsync(id);
            if (usageDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Usages.Remove(usageDelete);
            await _context.SaveChangesAsync();
        }
    }
}
