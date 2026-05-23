using Microsoft.AspNetCore.Mvc;
using SmartInventorySystem.Services;
using SmartInventorySystem.Models;
using SmartInventorySystem.Interfaces;

namespace SmartInventorySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedisController : ControllerBase
{
    private readonly RedisCacheService _cacheService;
    private readonly IProductRepository _repository;

    public RedisController(
        RedisCacheService cacheService,
        IProductRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        string cacheKey = "productList";

        var cachedProducts =
            await _cacheService
                .GetDataAsync<List<Product>>(cacheKey);

        if (cachedProducts != null)
        {
            return Ok(new
            {
                Source = "Redis Cache",
                Data = cachedProducts
            });
        }

        var products =
            await _repository.GetAllAsync();

        await _cacheService.SetDataAsync(
            cacheKey,
            products,
            TimeSpan.FromMinutes(5));

        return Ok(new
        {
            Source = "Database",
            Data = products
        });
    }
}