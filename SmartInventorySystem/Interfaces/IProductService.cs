using System.Collections.Generic;
using System.Threading.Tasks;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Models;

public interface IProductService
{
    Task<List<ProductDto>> GetAdvancedProducts(
    ProductQueryParameters parameters);
    Task<List<ProductDto>> GetProductsAdvanced(ProductQueryDto query);
    Task<List<ProductDto>> GetProducts();
    Task<List<LowStockProductDto>> GetLowStockProducts();
    Task<List<ProductDto>> SearchProducts(string keyword);
    Task<ProductDto> AddProduct(CreateProductDto dto);
    Task<List<ProductDto>> GetProductsSortedByPriceDesc(); // Add this method to match usage in ProductsController
    Task<List<ProductDto>> GetPagedProducts(
    int pageNumber,
    int pageSize);
    Task<ProductDto> GetProductById(int id);
    Task<DashboardDto> GetDashboardData();

    Task<List<CategorySummaryDto>> GetCategorySummary();
    Task DeleteProduct(int id);
    Task<ProductDto> UpdateProduct(
    int id,
    UpdateProductDto dto);

    Task SoftDeleteProduct(int id);

    Task RestoreProduct(int id);
}