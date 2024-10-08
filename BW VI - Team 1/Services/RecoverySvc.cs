﻿using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class RecoverySvc : IRecoverySvc
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

        public async Task<Recovery> AddRecoveryAsync(RecoveryDTO dto)
        {
            var animal = await _context.Animals.FindAsync(dto.AnimalId);
            if (animal == null)
            {
                throw new KeyNotFoundException("Animal not found");
            }

            var newRecovery = new Recovery
            {

                EndDate = dto.EndDate,
                Animal = animal,

            };

            _context.Recoverys.Add(newRecovery);
            await _context.SaveChangesAsync();
            return newRecovery;
        }


        public async Task<Recovery> UpdateRecoveryAsync(Recovery model)
        {
            var recovery = await _context.Recoverys.FindAsync(model.Id);
            if (recovery == null)
            {
                return null;
            }

            var animal = await _context.Animals.FindAsync(model.Animal.Id);
            if (animal == null)
            {
                throw new KeyNotFoundException("Animal not found");
            }

            recovery.StartDate = model.StartDate;
            recovery.EndDate = model.EndDate;
            recovery.Animal = animal;
            recovery.IsRefound = model.IsRefound;

            _context.Recoverys.Update(recovery);
            await _context.SaveChangesAsync();
            return recovery;
        }

        public async Task DeleteRecoveryAsync(int id)
        {
            var recovery = await _context.Recoverys.FindAsync(id);
            if (recovery == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Recoverys.Remove(recovery);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Recovery>> GetAllRecoveriesAsync()
            {
            return await _context.Recoverys.ToListAsync();
        }

        public async Task<Recovery> GetRecoveryByAnimal(int id)
            {
            return await _context.Recoverys.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
