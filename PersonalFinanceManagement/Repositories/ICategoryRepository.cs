using PersonalFinanceManagement.Models;
namespace PersonalFinanceManagement.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
    IEnumerable<Category> GetCategoriesByType(string type);

    Category GetCategoryById(int id);
    Task AddCategory(Category category);
    Task UpdateCategory(Category category);
    void DeleteCategory(int id);
}
