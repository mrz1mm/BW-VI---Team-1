using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class OrderSvc   : IOrderSvc
    {
        private readonly LifePetDBContext _context;
        public OrderSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
               .Include(o => o.Products)
               .Include(o => o.Owner)
               .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Owner)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddOrderAsync(OrderDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var selectedProducts = await _context.Products
                .Where(p => model.SelectedProductIds.Contains(p.Id))
                .ToListAsync();

            var newOrder = new Order
            {
                MedicalPrescription = model.MedicalPrescription,
                Date = model.Date,
                Products = selectedProducts,
                Owner = model.Owner 
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;
        }




        public async Task<Order> UpdateOrderAsync(Order model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var existingOrder = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Owner)
                .FirstOrDefaultAsync(o => o.Id == model.Id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            existingOrder.MedicalPrescription = model.MedicalPrescription;
            existingOrder.Date = model.Date;

            if (model.Products != null && model.Products.Any())
            {
                var productEntities = await _context.Products
                    .Where(p => model.Products.Select(pr => pr.Id).Contains(p.Id))
                    .ToListAsync();

                existingOrder.Products = productEntities;
            }

            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();
            return existingOrder;
        }


        public async Task DeleteOrderAsync(int id)
        {
            var OrderDelete = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (OrderDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Orders.Remove(OrderDelete);
            await _context.SaveChangesAsync();
        }
    }
}
