using System.Linq.Expressions;

namespace SmartInventorySystem.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<List<T>> GetAll();

    Task<T> GetById(int id);

    Task<T> Add(T entity);

    Task Update(T entity);

    Task Delete(T entity);

    Task<List<T>> Find(
        Expression<Func<T, bool>> predicate);
}
