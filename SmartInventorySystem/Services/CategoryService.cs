using AutoMapper;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;

namespace SmartInventorySystem.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Category> AddCategory(Category category)
    {
        return await _repository.AddCategory(category);
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _repository.GetCategories();
    }
    public async Task DeleteCategory(int id)
    {
        await _repository.DeleteCategory(id);
    }
    public async Task<Category> UpdateCategory(int id, Category category)
    {
        return await _repository.UpdateCategory(id, category);
    }
}