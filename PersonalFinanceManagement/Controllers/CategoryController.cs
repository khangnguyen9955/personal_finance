using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
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
        var categories = _categoryRepo.GetAllCategories().ToList();
        return View(categories);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


[HttpPost]
public IActionResult Create(Category category)
{
    _categoryRepo.AddCategory(category);
    return RedirectToAction(nameof(Index));
}



    //...
}
