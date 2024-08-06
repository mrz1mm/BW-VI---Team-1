using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Visits.Include(v => v.Animal).ToListAsync();
        }

        public async Task<Visit> GetVisitByIdAsync(int id)
        {
            return await _context.Visits.Include(v => v.Animal).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Visit> AddVisitAsync(VisitDTO model)
        {
            var animal = await _context.Animals.FindAsync(model.AnimalId);
            if (animal == null)
            {
                throw new ArgumentException("Animal not found");
            }

            var newVisit = new Visit
            {
                Date = model.Date,
                Exam = model.Exam,
                Diagnosis = model.Diagnosis,
                Animal = animal
            };

            _context.Visits.Add(newVisit);
            await _context.SaveChangesAsync();
            return newVisit;
        }


        public async Task<Visit> UpdateVisitAsync(Visit model)
        {
            var visit = await _context.Visits.FindAsync(model.Id);
            if (visit == null)
            {
                return null;
            }
            var animal = await _context.Animals.FindAsync(model.Animal.Id);
            if (animal == null)
            {
                throw new ArgumentException("Animal not found.");
            }

            visit.Date = model.Date;
            visit.Exam = model.Exam;
            visit.Diagnosis = model.Diagnosis;
            visit.Animal = animal;

            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
        }
    }
}
