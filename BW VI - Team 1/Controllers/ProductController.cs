using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using ProductType = BW_VI___Team_1.Models.Type;


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
            ViewBag.ProductTypes = new List<string> { "AnimalFood", "Medicine" };
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

            var model = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Suppliers = product.Suppliers,
                Type = product.Type,
                Usages = product.Usages,
                Locker = product.Locker
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
    }
}
