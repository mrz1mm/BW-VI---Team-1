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
        public async Task<IActionResult> UpdateLocker(int id)
        {
            var locker = await _lockerSvc.GetLockerByIdAsync(id);
            if (locker == null)
            {
                return NotFound();
            }

            var model = new LockerDTO
            {
                Number = locker.Number
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteLocker(int id)
        {
            try 
            {
                _lockerSvc.DeleteLockerAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
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
    }
}
