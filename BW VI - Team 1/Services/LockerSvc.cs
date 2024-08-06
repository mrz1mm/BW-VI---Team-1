using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class LockerSvc : ILockerSvc
    {
        private readonly LifePetDBContext _context;
        public LockerSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Locker>> GetAllLockersAsync() 
        {
            return await _context.Lockers.ToListAsync();
        }

        public async Task<Locker> GetLockerByIdAsync(int id) 
        {
            return await _context.Lockers.Include(l => l.Drawers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Locker> AddLockerAsync(LockerDTO model)
        {
            var newLocker = new Locker
            {
                Number = model.Number,
                Drawers = new List<Drawer>()
            };
            for (int i = 1; i <= 5; i++)
            {
                newLocker.Drawers.Add(new Drawer { Number = i });
            }
            _context.Lockers.Add(newLocker);
            await _context.SaveChangesAsync();
            return newLocker;

        }

        public async Task<Locker> UpdateLockerAsync(Locker model)
        {
            var locker = await _context.Lockers
                                       .Include(l => l.Drawers)
                                       .FirstOrDefaultAsync(l => l.Id == model.Id);
            if (locker == null)
            {
                throw new KeyNotFoundException();
            }

            locker.Number = model.Number;
            _context.Lockers.Update(locker);
            await _context.SaveChangesAsync();
            return locker;
        }

        public async Task DeleteLockerAsync(int id) 
        { 
            var locker = await _context.Lockers.FindAsync(id);

            if (locker == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Lockers.Remove(locker);
            await _context.SaveChangesAsync();

           
        }
    }
}
