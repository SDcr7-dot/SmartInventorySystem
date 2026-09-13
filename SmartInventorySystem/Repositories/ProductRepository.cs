using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Data;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

namespace SmartInventorySystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
    .Where(x => !x.IsDeleted)
    .ToListAsync();
        }
        public async Task<List<Product>> GetLowStockProducts()
        {
            return await _context.Products
                .Where(x => x.Quantity <= x.ReorderLevel)
                .ToListAsync();
        }
        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> SearchProducts(string keyword)
        {
            return await _context.Products
                .Where(x => x.Name.Contains(keyword))
                .ToListAsync();
        }
        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<List<Product>> GetAllProductsEntity()
        {
            return await _context.Products
                .Where(x => !x.IsDeleted)
                .Include(x => x.Category)
                .ToListAsync();
        }
        public async Task<Product> UpdateProduct(
    int id,
    Product product)
        {
            var existingProduct =
                await _context.Products.FindAsync(id);

            if (existingProduct == null)
                throw new Exception("Product not found");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.ReorderLevel = product.ReorderLevel;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return existingProduct;
        }
        public async Task SoftDeleteProduct(int id)
        {
            var product =
                await _context.Products.FindAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        public async Task RestoreProduct(int id)
        {
            var product =
                await _context.Products.FindAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.IsDeleted = false;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAdvancedProducts(ProductQueryParameters parameters)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(parameters.Search));
            }

            // Category Filter
            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryId ==
                    parameters.CategoryId.Value);
            }

            // Sorting
            query = parameters.SortBy switch
            {
                "price_asc" =>
                    query.OrderBy(x => x.Price),

                "price_desc" =>
                    query.OrderByDescending(x => x.Price),

                "name_desc" =>
                    query.OrderByDescending(x => x.Name),

                _ =>
                    query.OrderBy(x => x.Name)
            };

            // Pagination
            query = query
                .Skip((parameters.PageNumber - 1)
                    * parameters.PageSize)
                .Take(parameters.PageSize);

            return await query.ToListAsync();
        }
    }
}