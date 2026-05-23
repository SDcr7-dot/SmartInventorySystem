using Microsoft.Extensions.Caching.Memory;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

namespace SmartInventorySystem.Services;

public class CacheService
{
    private readonly IMemoryCache _cache;

    private readonly IProductRepository _repository;

    public CacheService(
        IMemoryCache cache,
        IProductRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<List<Product>> GetProducts()
    {
        string cacheKey = "products";

        if (!_cache.TryGetValue(
            cacheKey,
            out List<Product> products))
        {
            Console.WriteLine(
                "Fetching from database...");

            products =
                await _repository
                    .GetAllProductsEntity();

            var cacheOptions =
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(
                        TimeSpan.FromMinutes(5));

            _cache.Set(
                cacheKey,
                products,
                cacheOptions);
        }
        else
        {
            Console.WriteLine(
                "Fetching from cache...");
        }

        return products;
    }
}