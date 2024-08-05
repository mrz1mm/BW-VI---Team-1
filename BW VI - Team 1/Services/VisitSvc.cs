using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class VisitSvc : IVisitSvc
    {
        private readonly LifePetDBContext _context;
        public VisitSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Visit>> GetAllVisitsAsync()
        {
            return await _context.Visits.ToListAsync();
        }

        public async Task<Visit> GetVisitByIdAsync(int id)
        {
            return await _context.Visits.FindAsync(id);
        }

        public async Task<Visit> AddVisitAsync(VisitDTO model)
        {
            var newVisit = new Visit
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Visits.Add(newVisit);
            await _context.SaveChangesAsync();
            return newVisit;

        }

        public async Task<Visit> UpdateVisitAsync(Visit model)
        {
            var animal = await _context.Visits.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            // Aggiungere cose (es. animal.Name = model.Name)

            _context.Visits.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteVisitAsync(int id)
        {
            var animal = await _context.Visits.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Visits.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
