using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using BW_VI___Team_1.Services;
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
        public async Task<IActionResult> Index()
        {
            var usages = await _usageSvc.GetAllUsagesAsync();
            return View(usages);
        }

        [HttpGet]
        public IActionResult AddUsage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsage(int id)
        {
            var usage = await _usageSvc.GetUsageByIdAsync(id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsage(int id)
        {
            try
            {
                await _usageSvc.DeleteUsageAsync(id);
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
        public async Task<IActionResult> AddUsage([Bind("Description,Products")] UsageDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _usageSvc.AddUsageAsync(model);
                TempData["Success"] = "Usage aggiunto con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'usage";
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUsage(int id, [Bind("Description,Products")] Usage model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                var dto = new UsageDTO
                {
                    Description = model.Description
                };
                await _usageSvc.UpdateUsageAsync(dto, id);
                TempData["Success"] = "Usage modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'usage";
                return View(model);
            }
        }
    }
}
