using SmartInventorySystem.Data;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;
namespace SmartInventorySystem.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<Product>
        Products
    { get; }

    public IGenericRepository<Category>
        Categories
    { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Products =
            new GenericRepository<Product>(_context);

        Categories =
            new GenericRepository<Category>(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}