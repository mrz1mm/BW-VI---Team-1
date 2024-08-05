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
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> AddOrderAsync(OrderDTO model)
        {
            var newOrder = new Order
            {
                // Aggiungere cose (es. Name = model.Name)
            };
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;

        }

        public async Task<Order> UpdateOrderAsync(Order model)
        {
            var animal = await _context.Orders.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            // Aggiungere cose (es. animal.Name = model.Name)

            _context.Orders.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var animal = await _context.Orders.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Orders.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
