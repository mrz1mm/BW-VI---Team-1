using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class SupplierController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly ISupplierSvc _supplierSvc;
        public SupplierController(LifePetDBContext context, ISupplierSvc supplierSvc)
        {
            _context = context;
            _supplierSvc = supplierSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var suppliers = _supplierSvc.GetAllSuppliersAsync();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateSupplier(int id)
        {
            var supplier = _supplierSvc.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            var model = new Supplier
            {
                // aggiungere cose (es. Name = supplier.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _supplierSvc.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            var model = new Supplier
            {
                // aggiungere cose (es. Name = supplier.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSupplier(SupplierDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _supplierSvc.AddSupplierAsync(model);
                TempData["Success"] = "Suppliere aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'suppliere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSupplier(Supplier model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _supplierSvc.UpdateSupplierAsync(model);
                TempData["Success"] = "Suppliere modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'suppliere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteSupplier(int id)
        {
            try
            {
                await _supplierSvc.DeleteSupplierAsync(id);
                TempData["Success"] = "Suppliere eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'suppliere";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
