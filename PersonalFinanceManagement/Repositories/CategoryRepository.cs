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

    public Category GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
