﻿using BW_VI___Team_1.Interfaces;
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
            return await _context.Lockers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Locker> AddLockerAsync(LockerDTO model)
        {
            var newLocker = new Locker
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Lockers.Add(newLocker);
            await _context.SaveChangesAsync();
            return newLocker;

        }

        public async Task<Locker> UpdateLockerAsync(Locker model)
        {
            var locker = await _context.Lockers.FindAsync(model.Id);
            if (locker == null)
            {
                return null;
            }

            // Aggiungere cose (es. locker.Name = model.Name)

            _context.Lockers.Update(locker);
            await _context.SaveChangesAsync();
            return locker;
        }

        public async Task<bool> DeleteLockerAsync(int id) 
        { 
            var locker = await _context.Lockers.FindAsync(id);

            if (locker == null)
            {
                return false;
            }

            _context.Lockers.Remove(locker);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
