using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisitSvc _visitSvc;
        private readonly LifePetDBContext _context;

        public VisitController(IVisitSvc visitSvc, LifePetDBContext context)
        {
            _visitSvc = visitSvc;
            _context = context;
        }

        // VISTE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var visits = await _visitSvc.GetAllVisitsAsync();
            return View(visits);
        }

        [HttpGet]
        public IActionResult AddVisit()
        {
            // Popola ViewBag.Animals con l'elenco degli animali
            ViewBag.Animals = _context.Animals.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVisit(VisitDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Popola di nuovo ViewBag.Animals in caso di errore di validazione
                ViewBag.Animals = _context.Animals.ToList();
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
                // Popola di nuovo ViewBag.Animals in caso di eccezione
                ViewBag.Animals = _context.Animals.ToList();
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

            // Popola ViewBag.Animals con l'elenco degli animali
            ViewBag.Animals = _context.Animals.ToList();

            var model = new VisitDTO
            {
                Date = visit.Date,
                Exam = visit.Exam,
                Diagnosis = visit.Diagnosis,
                Animal = visit.Animal
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVisit(VisitDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Popola di nuovo ViewBag.Animals in caso di errore di validazione
                ViewBag.Animals = _context.Animals.ToList();
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                var visit = await _visitSvc.GetVisitByIdAsync(model.Id);
                if (visit == null)
                {
                    return NotFound();
                }

                visit.Date = model.Date;
                visit.Exam = model.Exam;
                visit.Diagnosis = model.Diagnosis;
                visit.Animal = model.Animal;

                await _visitSvc.UpdateVisitAsync(visit);
                TempData["Success"] = "Visita modificata con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Popola di nuovo ViewBag.Animals in caso di eccezione
                ViewBag.Animals = _context.Animals.ToList();
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica della visita";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _visitSvc.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteVisit(int id)
        {
            try
            {
                await _visitSvc.DeleteVisitAsync(id);
                TempData["Success"] = "Visita eliminata con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione della visita";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
