using SmartInventorySystem.Models;
using SmartInventorySystem.DTOs;

namespace SmartInventorySystem.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAdvancedProducts(
    ProductQueryParameters parameters);
        Task<Product> UpdateProduct(int id, Product product);

        Task SoftDeleteProduct(int id);

        Task RestoreProduct(int id);
        Task<List<Product>> GetAllAsync();

        Task<Product> AddAsync(Product product);
        Task<List<Product>> SearchProducts(string keyword);

        Task<List<Product>> GetLowStockProducts();
        Task<Product?> GetById(int id);
        Task<List<Product>> GetAllProductsEntity();
        Task DeleteProduct(int id);
    }
}