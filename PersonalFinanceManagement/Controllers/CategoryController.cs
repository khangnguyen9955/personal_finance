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
    public IActionResult CreateOrEdit(int id = 0)
    {
        if (id == 0)
        {
            return View(new Category());
        }
        else
        {
            return View(_categoryRepo.GetCategoryById(id));
            
        }
    }
    
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> CreateOrEdit([Bind("Id,Name,Type")] Category category)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         if (category.Id == 0)
    //              _categoryRepo.AddCategory(category);
    //         else
    //              _categoryRepo.UpdateCategory(category);
    //         return RedirectToAction(nameof(Index));
    //     }
    //     return View(category);
    // }
    //
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit([Bind("Id,Name,Type")] Category category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (category == null)
                {
                    return BadRequest("Invalid data supplied.");
                }

                if (category.Id == 0)
                {
                    await _categoryRepo.AddCategory(category);
                }
                else
                {
                    var categoryExist = _categoryRepo.GetCategoryById(category.Id);
                    if (categoryExist == null)
                    {
                        return NotFound("Category not found.");
                    }

                    categoryExist.Name = category.Name;
                    categoryExist.Type = category.Type;
                    await _categoryRepo.UpdateCategory(categoryExist);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        catch (Exception ex)
        {
            return BadRequest("An unexpected error occurred while processing your request." + ex.Message);
        }
    }



    //...
}
