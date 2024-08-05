using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IProductSvc
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductDTO product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
