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
                Date = model.Date,
                Exam = model.Exam,
                Diagnosis = model.Diagnosis,
                Animal = model.Animal
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

            visit.Date = model.Date;
            visit.Exam = model.Exam;
            visit.Diagnosis = model.Diagnosis;
            visit.Animal = model.Animal;

            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task<bool> DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return false;
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
