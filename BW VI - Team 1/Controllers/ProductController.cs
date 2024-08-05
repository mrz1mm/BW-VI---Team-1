using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
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
            var products = await _productSvc.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
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

            var model = new ProductDTO
            {
                Name = product.Name,
                Suppliers = product.Suppliers,
                Type = product.Type,
                Usages = product.Usages,
                Locker = product.Locker
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productSvc.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
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
        public async Task<IActionResult> UpdateProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                var product = await _productSvc.GetProductByIdAsync(model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                product.Name = model.Name;
                product.Suppliers = model.Suppliers;
                product.Type = model.Type;
                product.Usages = model.Usages;
                product.Locker = model.Type == Models.Type.Medicine ? model.Locker : null;

                await _productSvc.UpdateProductAsync(product);
                TempData["Success"] = "Prodotto modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica del prodotto";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteProduct(int id)
        {
            try
            {
                await _productSvc.DeleteProductAsync(id);
                TempData["Success"] = "Prodotto eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione del prodotto";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
