using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class UsageController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IUsageSvc _usageSvc;
        public UsageController(LifePetDBContext context, IUsageSvc usageSvc)
        {
            _context = context;
            _usageSvc = usageSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var usages = _usageSvc.GetAllUsagesAsync();
            return View(usages);
        }

        [HttpGet]
        public IActionResult AddUsage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateUsage(int id)
        {
            var usage = _usageSvc.GetUsageByIdAsync(id);
            if (usage == null)
            {
                return NotFound();
            }

            var model = new Usage
            {
                // aggiungere cose (es. Name = usage.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteUsage(int id)
        {
            var usage = _usageSvc.GetUsageByIdAsync(id);
            if (usage == null)
            {
                return NotFound();
            }

            var model = new Usage
            {
                // aggiungere cose (es. Name = usage.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsage(UsageDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _usageSvc.AddUsageAsync(model);
                TempData["Success"] = "Usagee aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'usagee";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUsage(Usage model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _usageSvc.UpdateUsageAsync(model);
                TempData["Success"] = "Usagee modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'usagee";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteUsage(int id)
        {
            try
            {
                await _usageSvc.DeleteUsageAsync(id);
                TempData["Success"] = "Usagee eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'usagee";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
