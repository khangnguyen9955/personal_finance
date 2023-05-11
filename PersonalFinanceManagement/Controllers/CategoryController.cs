using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;
using PersonalFinanceManagement.ViewModels;

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
    // [HttpGet]
    // public IActionResult CreateOrEdit(int id = 0)
    // {
    //     if (id == 0)
    //     {
    //         return View(new Category());
    //     }
    //     else
    //     {
    //         return View(_categoryRepo.GetCategoryById(id));
    //         
    //     }
    // }
    [HttpGet]
    public IActionResult CreateOrEdit(int id = 0)
    {
        var category = new Category();

        if (id != 0)
        {
            category = _categoryRepo.GetCategoryById(id);
        }
        else
        {
            category.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        return View(category);
    }

    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(CreateCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var category = new Category
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                UserId = userId,
            };

            if (model.Id == 0)
            {
                await _categoryRepo.AddCategory(category);
            }
            else
            {
                var categoryExist = _categoryRepo.GetCategoryById(model.Id);
                if (categoryExist == null)
                {
                    return NotFound("Category not found.");
                }

                categoryExist.Name = model.Name;
                categoryExist.Type = model.Type;
                await _categoryRepo.UpdateCategory(categoryExist);
            }

            return RedirectToAction(nameof(Index));
        }

        return View();
    }





    //...
}
