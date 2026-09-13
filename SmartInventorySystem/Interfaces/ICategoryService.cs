using SmartInventorySystem.Models;

namespace SmartInventorySystem.Interfaces;
public interface ICategoryService
{
    Task<List<Category>> GetCategories();
    Task<Category> AddCategory(Category category);
    Task DeleteCategory(int id);
    Task<Category> UpdateCategory(int id, Category category);
}