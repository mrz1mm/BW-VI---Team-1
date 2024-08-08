using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Controllers
{
    public class OwnerController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IOwnerSvc _ownerSvc;
        public OwnerController(LifePetDBContext context, IOwnerSvc ownerSvc)
        {
            _context = context;
            _ownerSvc = ownerSvc;
        }

        // VISTE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var owners = await _ownerSvc.GetAllOwnersAsync();
            return View(owners);
        }

        [HttpGet]
        public IActionResult AddOwner()
        {
            return View();
        }

        public IActionResult SearchBy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string fiscalCode)
        {
            if (string.IsNullOrEmpty(fiscalCode))
            {
                return PartialView("_SearchResults", null);
            }
            var owner = await _context.Owners
                .Include(o => o.Animals)
                .Where(o => o.FiscalCode == fiscalCode)
                .FirstOrDefaultAsync();

            return PartialView("_SearchResults", owner);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOwner(int id)
        {
            var owner = await _ownerSvc.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }

            var model = new Owner
            {
                Id = owner.Id,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                FiscalCode = owner.FiscalCode
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            try
            {
                await _ownerSvc.DeleteOwnerAsync(id);
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
        public async Task<IActionResult> AddOwner(OwnerDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _ownerSvc.AddOwnerAsync(model);
                TempData["Success"] = "Ownere aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'ownere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOwner(Owner model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _ownerSvc.UpdateOwnerAsync(model);
                TempData["Success"] = "Ownere modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'ownere";
                return View(model);
            }
        }
    }
}