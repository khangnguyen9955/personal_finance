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

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

      

        var categories = await _categoryRepo.GetAllCategoriesByUserId(user.Id);

        var categoryViewModels = categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, Type = c.Type }).ToList();

        return View(categoryViewModels);
    }


    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(Guid id)
    {
        var category = new Category();

        if (id != Guid.Empty)
        {
            category = _categoryRepo.GetCategoryById(id);
        }
        else
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            category.UserId = user.Id;
        }

        return View(category);
    }


    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(CategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception("User is not authenticated.");
            }

            var category = new Category
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                UserId = user.Id,
            };

            if (model.Id == Guid.Empty)
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
    public async Task<IActionResult> DeleteConfirmed(Guid id)
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



}
