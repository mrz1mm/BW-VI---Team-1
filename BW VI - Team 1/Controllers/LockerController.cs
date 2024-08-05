using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class LockerController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly ILockerSvc _lockerSvc;
        public LockerController(LifePetDBContext context, ILockerSvc lockerSvc)
        {
            _context = context;
            _lockerSvc = lockerSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var lockers = _lockerSvc.GetAllLockersAsync();
            return View(lockers);
        }

        [HttpGet]
        public IActionResult AddLocker()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateLocker(int id)
        {
            var locker = _lockerSvc.GetLockerByIdAsync(id);
            if (locker == null)
            {
                return NotFound();
            }

            var model = new Locker
            {
                // aggiungere cose (es. Name = locker.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteLocker(int id)
        {
            var locker = _lockerSvc.GetLockerByIdAsync(id);
            if (locker == null)
            {
                return NotFound();
            }

            var model = new Locker
            {
                // aggiungere cose (es. Name = locker.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLocker(LockerDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _lockerSvc.AddLockerAsync(model);
                TempData["Success"] = "Lockere aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'lockere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLocker(Locker model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _lockerSvc.UpdateLockerAsync(model);
                TempData["Success"] = "Lockere modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'lockere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteLocker(int id)
        {
            try
            {
                await _lockerSvc.DeleteLockerAsync(id);
                TempData["Success"] = "Lockere eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'lockere";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
