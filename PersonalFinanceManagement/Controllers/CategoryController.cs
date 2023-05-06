using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;

public class CategoryController: Controller
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryController(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public IActionResult Index()
    {
        var categories = _categoryRepo.GetAllCategories();
        return View(categories);
    }

    //...
}
