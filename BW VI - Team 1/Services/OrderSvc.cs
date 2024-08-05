using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class OrderSvc : IOrderSvc
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




        public async Task UpdateOrderAsync(int id, OrderDTO dto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Owner)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            existingOrder.MedicalPrescription = dto.MedicalPrescription;
            existingOrder.Date = dto.Date;

            if (dto.SelectedProductIds != null && dto.SelectedProductIds.Any())
            {
                var productEntities = await _context.Products
                    .Where(p => dto.SelectedProductIds.Contains(p.Id))
                    .ToListAsync();

                existingOrder.Products = productEntities;
            }
            else
            {
                existingOrder.Products.Clear();
            }

            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();
        }





        public async Task DeleteOrderAsync(int id)
        {
            var orderToDelete = await _context.Orders
                .Include(o => o.Products) 
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderToDelete == null)
            {
                throw new KeyNotFoundException();
            }
            _context.Products.RemoveRange(orderToDelete.Products);
            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();
        }

    }
}
