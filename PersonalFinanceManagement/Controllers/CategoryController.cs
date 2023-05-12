using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;
using PersonalFinanceManagement.ViewModels;

namespace PersonalFinanceManagement.Controllers;

[Authorize]
public class CategoryController: Controller
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly UserManager<User> _userManager;

  
    public CategoryController(ICategoryRepository categoryRepo, UserManager<User> userManager)
    {
        _userManager = userManager;
        _categoryRepo = categoryRepo;
    }

    public IActionResult Index()
    {
        string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var categories = _categoryRepo.GetAllCategories()
            .Where(c => c.UserId == currentUserId)
            .Select(c => new  CategoryViewModel{ Id = c.Id, Name = c.Name, Type = c.Type })
            .ToList();
        return View(categories);
    }

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
    public async Task<IActionResult> CreateOrEdit(CategoryViewModel model)
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
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var category = _categoryRepo.GetCategoryById(id);
        if (category == null || category.UserId != user.Id)
        {
            return NotFound();
        }

        _categoryRepo.DeleteCategory(id);
        return RedirectToAction(nameof(Index));
    }
    
    
    
    // " 1 + 2 * 3 / 4 * 5 + 3 * 2" /
    // [1,  + ,  2 * 3 / 4 * 5, + , 3 * 2  ]
        



    //...
}
