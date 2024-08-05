using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class OrderController : Controller
    {
        private readonly LifePetDBContext _context;
        private readonly IOrderSvc _orderSvc;
        public OrderController(LifePetDBContext context, IOrderSvc orderSvc)
        {
            _context = context;
            _orderSvc = orderSvc;
        }

        // VISTE
        [HttpGet]
        public IActionResult Index()
        {
            var orders = _orderSvc.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateOrder(int id)
        {
            var order = _orderSvc.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new Order
            {
                // aggiungere cose (es. Name = order.Name)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orderSvc.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new Order
            {
                // aggiungere cose (es. Name = order.Name)
            };

            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(OrderDTO model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _orderSvc.AddOrderAsync(model);
                TempData["Success"] = "Ordere aggiunto con successo";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nell'aggiunta dell'ordere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(Order model) // aggiungere il Binding
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nella compilazione dei campi";
                return View(model);
            }

            try
            {
                await _orderSvc.UpdateOrderAsync(model);
                TempData["Success"] = "Ordere modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'ordere";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteOrder(int id)
        {
            try
            {
                await _orderSvc.DeleteOrderAsync(id);
                TempData["Success"] = "Ordere eliminato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore nell'eliminazione dell'ordere";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
