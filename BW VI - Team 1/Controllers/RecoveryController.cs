using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var recoverys = _recoverySvc.GetAllRecoveriesAsync();
            return View(recoverys);
        }

        [HttpGet]
        public IActionResult AddRecovery()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateRecovery(int id)
        {
            var recovery = _recoverySvc.GetRecoveryByIdAsync(id);
            if (recovery == null)
            {
                return NotFound();
            }

            var model = new Recovery
            {
                // aggiungere cose (es. Name = recovery.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteRecovery(int id)
        {
            var recovery = _recoverySvc.GetRecoveryByIdAsync(id);
            if (recovery == null)
            {
                return NotFound();
            }

            var model = new Recovery
            {
                // aggiungere cose (es. Name = recovery.Name)
            };

            return View();
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
                await _recoverySvc.AddRecoveryAsync(model);
                TempData["Success"] = "Recoverye aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'recoverye";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteRecovery(int id)
        {
            try
            {
                await _recoverySvc.DeleteRecoveryAsync(id);
                TempData["Success"] = "Recoverye eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'recoverye";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
