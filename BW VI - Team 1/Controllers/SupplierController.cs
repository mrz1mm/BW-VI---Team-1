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
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierSvc.GetAllSuppliersAsync();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id)
        {
            var supplier = await _supplierSvc.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _supplierSvc.DeleteSupplierAsync(id);
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
        public async Task<IActionResult> AddSupplier( SupplierDTO model) 
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _supplierSvc.AddSupplierAsync(model);
                TempData["Success"] = "Fornitore aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta del fornitore";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSupplier(Supplier model) 
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _supplierSvc.UpdateSupplierAsync(model);
                TempData["Success"] = "Fornitore modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica del fornitore";
                return View(model);
            }
        }
    }
}

