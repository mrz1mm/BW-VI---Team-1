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
            if (model.Locker != null)
            {
                var existingLocker = await _context.Lockers
                    .FirstOrDefaultAsync(l => l.Number == model.Locker.Number);

                if (existingLocker != null)
                {
                    newProduct.Locker = existingLocker;
                }
                else
                {
                    newProduct.Locker = model.Locker;
                    _context.Lockers.Add(newProduct.Locker);
                }
            }

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }


        public async Task<Product> UpdateProductAsync(Product model)
        {
            var product = await _context.Products
                .Include(p => p.Suppliers)
                .Include(p => p.Usages)
                .Include(p => p.Locker)
                .Include(p => p.Drawer)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (product == null)
            {
                return null;
            }

            product.Name = model.Name;
            product.Type = model.Type;
            product.LockerId = model.LockerId;
            product.Locker = await _context.Lockers.FindAsync(model.LockerId);
            product.DrawerId = model.DrawerId;
            product.Drawer = await _context.Drawers.FindAsync(model.DrawerId);

            product.Suppliers.Clear();
            if (model.Suppliers != null)
            {
                foreach (var supplier in model.Suppliers)
                {
                    var supplierToAdd = await _context.Suppliers.FindAsync(supplier.Id);
                    if (supplierToAdd != null)
                    {
                        product.Suppliers.Add(supplierToAdd);
                    }
                }
            }
            product.Usages.Clear();
            if (model.Usages != null)
            {
                foreach (var usage in model.Usages)
                {
                    var usageToAdd = await _context.Usages.FindAsync(usage.Id);
                    if (usageToAdd != null)
                    {
                        product.Usages.Add(usageToAdd);
                    }
                }
            }
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
