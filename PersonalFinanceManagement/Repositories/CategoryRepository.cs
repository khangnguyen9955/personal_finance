using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MyDbContext _context;

    public CategoryRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _context.Categories.ToList();
    }

    public IEnumerable<Category> GetCategoriesByType(string type)
    {
        return _context.Categories.Where(c => c.Type == type);
    }
    public Category GetCategoryById(Guid id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    // public async void AddCategory(Category category)
    // {
    //  
    //     _context.Categories.Add(category);
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async void UpdateCategory(Category category)
    // {
    //     _context.Categories.Update(category);
    //     await _context.SaveChangesAsync();
    // }
    
    public async Task AddCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }


    public void DeleteCategory(Guid id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
    
    public async Task<List<Category>> GetAllCategoriesByUserId(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

}
