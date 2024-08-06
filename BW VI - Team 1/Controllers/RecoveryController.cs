using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Controllers
{
    public class RecoveryController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IRecoverySvc _recoverySvc;
        public RecoveryController(LifePetDBContext context, IRecoverySvc recoverySvc)
        {
            _context = context;
            _recoverySvc = recoverySvc;
        }

        // VISTE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var recoveries = await _context.Recoverys
                .Include(r => r.Animal)
                .ToListAsync();

            return View(recoveries);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecovery()
        {
            var animals = await _context.Animals.ToListAsync();
            ViewBag.Animals = new SelectList(animals, "Id", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRecovery(int id)
        {
            var recovery = await _recoverySvc.GetRecoveryByIdAsync(id);
            if (recovery == null)
            {
                return NotFound();
            }

            var animals = await _context.Animals.ToListAsync();
            ViewBag.Animals = new SelectList(animals, "Id", "Name");

            return View(recovery);
        }



        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecovery(RecoveryDTO model) // aggiungere il Binding
        {
            try
            {
                var animal = await _context.Animals.FindAsync(model.AnimalId);

                if (animal == null)
                {
                    TempData["Error"] = "Animale non trovato";
                    return View(model);
                }

                var newRecovery = new Recovery
                {

                    EndDate = model.EndDate,
                    Animal = animal,

                };

                _context.Recoverys.Add(newRecovery);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Recovery aggiunto con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta del recovery";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecovery(Recovery model)
        {
            try
            {
                var animal = await _context.Animals.FindAsync(model.AnimalId);
                if (animal == null)
                {
                    ModelState.AddModelError("", "Animal not found");
                    return View(model);
                }

                model.Animal = animal;
                await _recoverySvc.UpdateRecoveryAsync(model);
                TempData["Success"] = "Recovery modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica del recovery";
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecovery(int id)
        {
            try
            {
                await _recoverySvc.DeleteRecoveryAsync(id);
                TempData["Success"] = "Recovery eliminato con successo";
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Recovery non trovato";
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Errore durante la cancellazione: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
