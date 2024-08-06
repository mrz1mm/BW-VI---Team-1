using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisitSvc _visitSvc;
        private readonly LifePetDBContext _context;
        private readonly IAnimalSvc _animalSvc;

        public VisitController(IVisitSvc visitSvc, LifePetDBContext context, IAnimalSvc animalSvc)
        {
            _visitSvc = visitSvc;
            _context = context;
            _animalSvc = animalSvc;
        }

        // VISTE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var visits = await _visitSvc.GetAllVisitsAsync();
            return View(visits);
        }

        [HttpGet]
        public async Task<IActionResult> AddVisit()
        {
            var animals = await _animalSvc.GetAllAnimalsAsync();
            ViewBag.Animals = animals;

            return View(new VisitDTO { Date = DateOnly.FromDateTime(DateTime.Now) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVisit(VisitDTO model)
        {
            if (!ModelState.IsValid)
            {
                var animals = await _animalSvc.GetAllAnimalsAsync();
                ViewBag.Animals = animals;

                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _visitSvc.AddVisitAsync(model);
                TempData["Success"] = "Visita aggiunta con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var animals = await _animalSvc.GetAllAnimalsAsync();
                ViewBag.Animals = animals;

                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta della visita";
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> UpdateVisit(int id)
        {
            var visit = await _visitSvc.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            var model = new VisitDTO
            {
                Date = visit.Date,
                Exam = visit.Exam,
                Diagnosis = visit.Diagnosis,
                AnimalId = visit.Animal.Id 
            };
            ViewBag.VisitId = id;
            ViewBag.Animals = await _context.Animals.ToListAsync();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVisit(VisitDTO model, int id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Animals = await _context.Animals.ToListAsync();
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                var visit = await _visitSvc.GetVisitByIdAsync(id);
                if (visit == null)
                {
                    return NotFound();
                }

                visit.Date = model.Date;
                visit.Exam = model.Exam;
                visit.Diagnosis = model.Diagnosis;

                var animal = await _context.Animals.FindAsync(model.AnimalId);
                if (animal == null)
                {
                    TempData["Error"] = "Animale non trovato";
                    return View(model);
                }
                visit.Animal = animal;

                await _visitSvc.UpdateVisitAsync(visit);
                TempData["Success"] = "Visita modificata con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Animals = await _context.Animals.ToListAsync();
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica della visita";
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            try
            {
                await _visitSvc.DeleteVisitAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
