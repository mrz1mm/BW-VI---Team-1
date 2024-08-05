using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Services
{
    public class ProductSvc
    {
        private readonly LifePetDBContext _context;
        public ProductSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Suppliers).Include(p => p.Usages).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Suppliers).Include(p => p.Usages).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddProductAsync(ProductDTO model)
        {
            var newProduct = new Product
            {
                Name = model.Name,
                Suppliers = model.Suppliers,
                Type = model.Type,
                Usages = model.Usages,
                Locker = model.Type == Models.Type.Medicine ? model.Locker : null
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;

        }

        public async Task<Product> UpdateProductAsync(Product model)
        {
            var animal = await _context.Products.FindAsync(model.Id);
            if (animal == null)
            {
                return null;
            }

            animal.Name = model.Name;
            animal.Suppliers = model.Suppliers;
            animal.Type = model.Type;
            animal.Usages = model.Usages;
            animal.Locker = model.Type == Models.Type.Medicine ? model.Locker : null;

            _context.Products.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var animal = await _context.Products.FindAsync(id);
            if (animal == null)
            {
                return false;
            }

            _context.Products.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
