using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

namespace SmartInventorySystem.Services;

public class UnitOfWorkDemoService
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkDemoService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _unitOfWork
            .Products
            .GetAll();
    }
}