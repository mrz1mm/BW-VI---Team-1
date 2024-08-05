using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class OrderSvc
    {
        private readonly LifePetDBContext _context;
        public OrderSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Products).Include(o => o.Owner).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.Products).Include(o => o.Owner).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddOrderAsync(OrderDTO model)
        {
            var newOrder = new Order
            {
                Products = model.Products,
                Owner = model.Owner,
                MedicalPrescription = model.MedicalPrescription,
                Date = model.Date
            };
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;

        }

        public async Task<Order> UpdateOrderAsync(Order model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return null;
            }

            order.Products = model.Products;
            order.Owner = model.Owner;
            order.MedicalPrescription = model.MedicalPrescription;
            order.Date = model.Date;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
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
