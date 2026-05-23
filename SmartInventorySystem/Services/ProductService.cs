namespace SmartInventorySystem.Services;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<ProductDto>>
    GetAdvancedProducts(
    ProductQueryParameters parameters)
    {
        var products =
            await _repository
                .GetAdvancedProducts(parameters);

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetProducts()
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<LowStockProductDto>> GetLowStockProducts()
    {
        var products = await _repository.GetLowStockProducts();

        return _mapper.Map<List<LowStockProductDto>>(products);
    }

    public async Task<ProductDto> AddProduct(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);

        var result = await _repository.AddAsync(product);

        return _mapper.Map<ProductDto>(result);
    }

    public async Task<List<ProductDto>> SearchProducts(string keyword)
    {
        var products = await _repository.SearchProducts(keyword);

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetProductsSortedByPriceDesc()
    {
        var products = await _repository.GetAllAsync();

        var sortedProducts = products
            .OrderByDescending(p => p.Price)
            .ToList();

        return _mapper.Map<List<ProductDto>>(sortedProducts);
    }

    public async Task<List<ProductDto>> GetPagedProducts(
        int pageNumber,
        int pageSize)
    {
        var products = await _repository.GetAllAsync();

        var pagedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<ProductDto>>(pagedProducts);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var product = await _repository.GetById(id);

        if (product == null)
            throw new Exception("Product not found");

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<DashboardDto> GetDashboardData()
    {
        var products = await _repository.GetAllProductsEntity();

        return new DashboardDto
        {
            TotalProducts = products.Count,

            TotalInventoryValue =
                products.Sum(x => x.Price * x.Quantity),

            LowStockProducts =
                products.Count(x => x.Quantity <= x.ReorderLevel)
        };
    }

    public async Task<List<CategorySummaryDto>> GetCategorySummary()
    {
        var products = await _repository.GetAllProductsEntity();

        var result = products
            .GroupBy(x => x.Category.Name)
            .Select(x => new CategorySummaryDto
            {
                CategoryName = x.Key,
                ProductCount = x.Count()
            })
            .ToList();

        return result;
    }

    public async Task DeleteProduct(int id)
    {
        await _repository.DeleteProduct(id);
    }
    public async Task<ProductDto> UpdateProduct(
    int id,
    UpdateProductDto dto)
    {
        var product =
            _mapper.Map<Product>(dto);

        var result =
            await _repository.UpdateProduct(id, product);

        return _mapper.Map<ProductDto>(result);
    }
    public async Task SoftDeleteProduct(int id)
    {
        await _repository.SoftDeleteProduct(id);
    }
    public async Task RestoreProduct(int id)
    {
        await _repository.RestoreProduct(id);
    }
    public async Task<List<ProductDto>> GetProductsAdvanced(ProductQueryDto query)
    {
        var products = await _repository.GetAllProductsEntity();

        // SEARCH

        if (!string.IsNullOrEmpty(query.Search))
        {
            products = products
                .Where(x =>
                    x.Name.ToLower()
                    .Contains(query.Search.ToLower()))
                .ToList();
        }

        // FILTER

        if (query.CategoryId.HasValue)
        {
            products = products
                .Where(x => x.CategoryId == query.CategoryId.Value)
                .ToList();
        }

        // SORTING

        products = query.SortBy?.ToLower() switch
        {
            "price_desc" =>
                products.OrderByDescending(x => x.Price).ToList(),

            "price_asc" =>
                products.OrderBy(x => x.Price).ToList(),

            _ => products
        };

        // PAGINATION

        products = products
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return _mapper.Map<List<ProductDto>>(products);
    }
}