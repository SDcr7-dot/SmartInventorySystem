using SmartInventorySystem.Models;
namespace SmartInventorySystem.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Product> Products { get; }

    IGenericRepository<Category> Categories { get; }

    Task<int> SaveAsync();
}
