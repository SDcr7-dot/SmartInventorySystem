using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SmartInventorySystem.Data;
using SmartInventorySystem.Interfaces;

namespace SmartInventorySystem.Repositories;

public class GenericRepository<T>
    :IGenericRepository<T>
    where T : class
{
    private readonly AppDbContext _context;

    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;

        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task Update(T entity)
    {
        _dbSet.Update(entity);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> Find(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .ToListAsync();
    }
}
