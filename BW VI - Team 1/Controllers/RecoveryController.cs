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
            var recoveries = await _recoverySvc.GetAllRecoveriesAsync();
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
            var model = new RecoveryDTO
            {
                StartDate = recovery.StartDate,
                EndDate = recovery.EndDate,
                Animal = recovery.Animal,
                IsRefound = recovery.IsRefound
            };

            return View(model);
        }

        [HttpPost]
        public async Task DeleteRecovery(int id)
        {
            var recovery = await _context.Recoverys.FindAsync(id);
            if (recovery == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Recoverys.Remove(recovery);
            await _context.SaveChangesAsync();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecovery(RecoveryDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                var animal = await _context.Animals.FindAsync(model.Animal.Id);

                if (animal == null)
                {
                    TempData["Error"] = "Animale non trovato";
                    return View(model);
                }

                var newRecovery = new Recovery
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Animal = animal,
                    IsRefound = model.IsRefound
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
        public async Task<IActionResult> UpdateRecovery(Recovery model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _recoverySvc.UpdateRecoveryAsync(model);
                TempData["Success"] = "Recoverye modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'recoverye";
                return View(model);
            }
        }
    }
}
