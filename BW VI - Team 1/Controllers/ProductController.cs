using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductType = BW_VI___Team_1.Models.Type;

namespace BW_VI___Team_1.Controllers
{
    [Authorize(Policy = Policies.Pharmacist)]
    public class ProductController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IProductSvc _productSvc;
        public ProductController(LifePetDBContext context, IProductSvc productSvc)
        {
            _context = context;
            _productSvc = productSvc;
        }

        // VISTE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Suppliers)
                .Include(p => p.Usages)
                .Include(p => p.Locker)
                .ThenInclude(l => l.Drawers)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.ProductTypes = new List<string> { "AnimalFood", "Medicine" };
            ViewBag.Usages = await _context.Usages.ToListAsync();
            ViewBag.Suppliers = await _context.Suppliers.ToListAsync();
            ViewBag.Lockers = await _context.Lockers.ToListAsync();
            ViewBag.Drawers = await _context.Drawers.Take(5).ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productSvc.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductTypes = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(t => t.ToString()).ToList();
            ViewBag.Suppliers = await _context.Suppliers.ToListAsync();
            ViewBag.Usages = await _context.Usages.ToListAsync();
            ViewBag.Lockers = await _context.Lockers.ToListAsync();
            ViewBag.Drawers = await _context.Drawers.Take(5).ToListAsync();

            var model = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Suppliers = product.Suppliers,
                Type = product.Type,
                Usages = product.Usages,
                LockerId = product.Locker?.Id, 
                DrawerId = product.Drawer?.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productSvc.DeleteProductAsync(id);
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
        public async Task<IActionResult> AddProduct([Bind("Name,Type,Locker,DrawerId")] ProductDTO model, [Bind("Usages")] int[] Usages, [Bind("Suppliers")] int[] Suppliers)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                model.Usages = await _context.Usages.Where(u => Usages.Contains(u.Id)).ToListAsync();
                model.Suppliers = await _context.Suppliers.Where(s => Suppliers.Contains(s.Id)).ToListAsync();

                if (model.Type == Models.Type.Medicine)
                {
                    if (model.Locker != null)
                    {
                        var drawer = await _context.Drawers.FindAsync(model.DrawerId);
                        if (drawer != null)
                        {
                            model.Locker.Drawers = new List<Drawer> { drawer };
                        }
                    }
                }
                else
                {
                    model.Locker = null;
                    model.DrawerId = null;
                }
                await _productSvc.AddProductAsync(model);
                TempData["Success"] = "Prodotto aggiunto con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta del prodotto";
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct([Bind("Id,Name,Type,LockerId,DrawerId")] Product model, [Bind("Usages")] int[] Usages, [Bind("Suppliers")] int[] Suppliers)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                model.Usages = await _context.Usages.Where(u => Usages.Contains(u.Id)).ToListAsync();
                model.Suppliers = await _context.Suppliers.Where(s => Suppliers.Contains(s.Id)).ToListAsync();

                if (model.Type == Models.Type.Medicine)
                {
                    if (model.LockerId.HasValue)
                    {
                        var drawer = await _context.Drawers.FindAsync(model.DrawerId);
                        if (drawer != null)
                        {
                            model.Drawer = drawer; 
                        }
                    }
                }
                else
                {
                    model.LockerId = null;
                    model.DrawerId = null; 
                }

                await _productSvc.UpdateProductAsync(model);
                TempData["Success"] = "Prodotto modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = $"Errore nella modifica del prodotto: {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["Error"] += $" Inner Exception: {ex.InnerException.Message}";
                }
                return View(model);
            }
        }
    }
}

