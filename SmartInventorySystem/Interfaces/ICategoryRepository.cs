using SmartInventorySystem.Models;

namespace SmartInventorySystem.Interfaces;
    public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> AddAsync(Category category);
    Task<Category> AddCategory(Category category);
    Task<List<Category>> GetCategories();
    Task DeleteCategory(int id);
    Task<Category> UpdateCategory(int id, Category category);
}
