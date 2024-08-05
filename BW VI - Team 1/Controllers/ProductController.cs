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
        public IActionResult Index()
        {
            var products = _productSvc.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var product = _productSvc.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new Product
            {
                // aggiungere cose (es. Name = product.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productSvc.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new Product
            {
                // aggiungere cose (es. Name = product.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _productSvc.AddProductAsync(model);
                TempData["Success"] = "Producte aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'producte";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(Product model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _productSvc.UpdateProductAsync(model);
                TempData["Success"] = "Producte modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'producte";
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
                TempData["Success"] = "Producte eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'producte";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
