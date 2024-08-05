using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var owners = _ownerSvc.GetAllOwnersAsync();
            return View(owners);
        }

        [HttpGet]
        public IActionResult AddOwner()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateOwner(int id)
        {
            var owner = _ownerSvc.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }

            var model = new Owner
            {
                // aggiungere cose (es. Name = owner.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteOwner(int id)
        {
            var owner = _ownerSvc.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }

            var model = new Owner
            {
                // aggiungere cose (es. Name = owner.Name)
            };

            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteOwner(int id)
        {
            try
            {
                await _ownerSvc.DeleteOwnerAsync(id);
                TempData["Success"] = "Ownere eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'ownere";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}