using Microsoft.AspNetCore.Mvc;
using SmartInventorySystem.Services;

namespace SmartInventorySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitOfWorkDemoController
    : ControllerBase
{
    private readonly UnitOfWorkDemoService
        _service;

    public UnitOfWorkDemoController(
        UnitOfWorkDemoService service)
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