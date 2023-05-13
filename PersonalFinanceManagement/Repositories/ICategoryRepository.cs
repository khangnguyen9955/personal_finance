using PersonalFinanceManagement.Models;
namespace PersonalFinanceManagement.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
    Task<List<Category>> GetAllCategoriesByUserId(Guid userId);
   

    IEnumerable<Category> GetCategoriesByType(string type);

    Category GetCategoryById(Guid id);
    Task AddCategory(Category category);
    Task UpdateCategory(Category category);
    void DeleteCategory(Guid id);
    
    
}
