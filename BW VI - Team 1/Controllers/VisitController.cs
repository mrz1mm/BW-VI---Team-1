using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class VisitController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IVisitSvc _visitSvc;
        public VisitController(LifePetDBContext context, IVisitSvc visitSvc)
        {
            _context = context;
            _visitSvc = visitSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var visits = _visitSvc.GetAllVisitsAsync();
            return View(visits);
        }

        [HttpGet]
        public IActionResult AddVisit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateVisit(int id)
        {
            var visit = _visitSvc.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            var model = new Visit
            {
                // aggiungere cose (es. Name = visit.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteVisit(int id)
        {
            var visit = _visitSvc.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            var model = new Visit
            {
                // aggiungere cose (es. Name = visit.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVisit(VisitDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _visitSvc.AddVisitAsync(model);
                TempData["Success"] = "Visite aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'visite";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVisit(Visit model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _visitSvc.UpdateVisitAsync(model);
                TempData["Success"] = "Visite modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'visite";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteVisit(int id)
        {
            try
            {
                await _visitSvc.DeleteVisitAsync(id);
                TempData["Success"] = "Visite eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'visite";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
