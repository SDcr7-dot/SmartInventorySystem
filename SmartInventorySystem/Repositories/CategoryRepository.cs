namespace SmartInventorySystem.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Data;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> AddCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new Exception("Category not found");

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
    }
    public async Task<Category> UpdateCategory(int id, Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(id);

        if (existingCategory == null)
            throw new Exception("Category not found");

        existingCategory.Name = category.Name;

        await _context.SaveChangesAsync();

        return existingCategory;
    }
}