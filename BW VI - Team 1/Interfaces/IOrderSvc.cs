using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IOrderSvc
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> AddOrderAsync(OrderDTO order);
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
