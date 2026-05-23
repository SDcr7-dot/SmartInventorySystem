using Microsoft.AspNetCore.Mvc;
using SmartInventorySystem.Services;

namespace SmartInventorySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController : ControllerBase
{
    private readonly CacheService _service;

    public CacheController(
        CacheService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data =
            await _service.GetProducts();

        return Ok(data);
    }
}