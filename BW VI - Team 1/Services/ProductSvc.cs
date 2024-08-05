﻿using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using BW_VI___Team_1.Interfaces;

namespace BW_VI___Team_1.Services
{
    public class ProductSvc : IProductSvc
    {
        private readonly LifePetDBContext _context;
        public ProductSvc(LifePetDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
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
            var product = await _context.Products.FindAsync(model.Id);
            if (product == null)
            {
                return null;
            }

            product.Name = model.Name;
            product.Suppliers = model.Suppliers;
            product.Type = model.Type;
            product.Usages = model.Usages;
            product.Locker = model.Type == Models.Type.Medicine ? model.Locker : null;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var ProductDelete = await _context.Products.FindAsync(id);
            if (ProductDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Products.Remove(ProductDelete);
            await _context.SaveChangesAsync();
        }
    }
}
