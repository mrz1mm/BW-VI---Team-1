using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            var orders = await _orderSvc.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrder()
        {
            var products = await _context.Products.ToListAsync();
            var owners = await _context.Owners.ToListAsync();

            ViewBag.Products = products;
            ViewBag.Owners = owners.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.FirstName} {o.LastName}"
            }).ToList();

            var model = new OrderDTO
            {
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await _orderSvc.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Products = await _context.Products.ToListAsync();
            ViewBag.Owners = await _context.Owners.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.FirstName} {o.LastName}"
            }).ToListAsync();

            return View(order);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
           try
            {
                await _orderSvc.DeleteOrderAsync(id);
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
        public async Task<IActionResult> AddOrder(OrderDTO model, int[] selectedProductIds)
        {
            try
            {
                if (model.Owner == null || model.Owner.Id == 0)
                {
                    ModelState.AddModelError("", "Owner is required.");
                    return View(model);
                }

                var selectedOwner = await _context.Owners.FindAsync(model.Owner.Id);
                if (selectedOwner == null)
                {
                    ModelState.AddModelError("", "Owner not found.");
                    return View(model);
                }

                model.Owner = selectedOwner;

                if (selectedProductIds == null || !selectedProductIds.Any())
                {
                    ModelState.AddModelError("", "At least one product must be selected.");
                    return View(model);
                }

                model.SelectedProductIds = selectedProductIds; 

                await _orderSvc.AddOrderAsync(model);
                TempData["Success"] = "Order added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error adding order: {ex.Message}");
                TempData["Error"] = "Error adding order.";
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
                var order = await _orderSvc.GetOrderByIdAsync(model.Id);
                if (order == null)
                {
                    return NotFound();
                }

                order.MedicalPrescription = model.MedicalPrescription;
                order.Date = model.Date;
                order.Owner = model.Owner;

                if (model.Products != null && model.Products.Any())
                {
                    var productEntities = await _context.Products
                        .Where(p => model.Products.Select(prod => prod.Id).Contains(p.Id))
                        .ToListAsync();

                    order.Products = productEntities;
                }

                await _orderSvc.UpdateOrderAsync(order);
                TempData["Success"] = "Ordine modificato con successo";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = "Errore nella modifica dell'ordine";
                return View(model);
            }
        }


    }
}
