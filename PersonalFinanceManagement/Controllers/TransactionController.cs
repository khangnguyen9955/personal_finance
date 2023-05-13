using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;
using PersonalFinanceManagement.ViewModels;
using System.Collections.Generic;

using Syncfusion.EJ2.Linq;

namespace PersonalFinanceManagement.Controllers;
[Authorize]
public class TransactionController :  Controller
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IIncomeRepository _incomeRepo;
    private readonly ISpendingRepository _spendingRepo;
    private readonly UserManager<User> _userManager;

    public TransactionController( UserManager<User> userManager,ICategoryRepository categoryRepo, IIncomeRepository incomeRepo, ISpendingRepository spendingRepo)
    {
        _categoryRepo = categoryRepo;
        _incomeRepo = incomeRepo;
        _spendingRepo = spendingRepo;
        _userManager = userManager;

    }

    public IActionResult Index()
    {
        // var incomes = _incomeRepo.GetAllIncomes().ToList(); // convert to list
        // var spendings = _spendingRepo.GetAllSpendings().ToList();
        // var viewModel = new TransactionViewModel 
        // {
        //     Incomes = incomes,
        //     Spendings = spendings,
        //     Categories = _categoryRepo.GetAllCategories().ToList()
        // };
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(Guid id) 
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User is not authenticated.");
        }

        var categories = await _categoryRepo.GetAllCategoriesByUserId(user.Id);
        var viewModel = new TransactionViewModel
        {
            Categories = categories
        };

        if (id == Guid.Empty) // Create new transaction
        {
            viewModel.TransactionType = "income"; // Default to "income" for new transaction
        }
        else {
            var spending = _spendingRepo.GetSpendingById(id);
            if (spending != null)
            {
                viewModel.Id = spending.Id;
                viewModel.CategoryId = spending.CategoryId;
                viewModel.Amount = spending.Amount; 
                viewModel.Description = spending.Description;
                viewModel.Date = spending.Date;
                viewModel.TransactionType = "spending";
            }
            else
            {
                var income = _incomeRepo.GetIncomeById(id);
                if (income != null)
                {
                    viewModel.Id = income.Id;
                    viewModel.CategoryId = income.CategoryId;
                    viewModel.Amount = income.Amount;
                    viewModel.Description = income.Description;
                    viewModel.Date = income.Date;
                    viewModel.TransactionType = "income";
                }
                else
                {
                    return NotFound();
                }
            }
        }

        return View(viewModel);
    }



 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateOrEdit(TransactionViewModel transactionViewModel, Guid id)
{ var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        throw new Exception("User is not authenticated.");
    }

    var categories = await _categoryRepo.GetAllCategoriesByUserId(user.Id);
    
    if (id == Guid.Empty )// Create new transaction
    {
        if (ModelState.IsValid)
        {
            if (transactionViewModel.TransactionType == "income")
            {
                // create new Income object
                var income = new Income
                {
                    Amount = transactionViewModel.Amount,
                    CategoryId = transactionViewModel.CategoryId,
                    Date = transactionViewModel.Date,
                    Description = transactionViewModel.Description,
                    UserId = Guid.Parse(_userManager.GetUserId(User))
                };

                // add income to repository
                _incomeRepo.AddIncome(income);
            }
            else if (transactionViewModel.TransactionType == "spending")
            {
                // create new Spending object
                var spending = new Spending
                {
                    Amount = transactionViewModel.Amount,
                    CategoryId = transactionViewModel.CategoryId,
                    Date = transactionViewModel.Date,
                    Description = transactionViewModel.Description,
                    UserId = Guid.Parse(_userManager.GetUserId(User))
                };

                // add spending to repository
                _spendingRepo.AddSpending(spending);
            }
            else
            {
                // Invalid transaction type
                return BadRequest();
            }
           
            transactionViewModel.Categories = categories;
            return RedirectToAction("Index");
        }
    }
    else // Edit existing transaction
    {
        // Determine whether the transaction being edited is an income or a spending
        var income =  _incomeRepo.GetIncomeById(id);
        var spending =_spendingRepo.GetSpendingById(id);

        if (income != null)
        {
            if (ModelState.IsValid)
            {
                income.Amount = transactionViewModel.Amount;
                income.CategoryId = transactionViewModel.CategoryId;
                income.Date = transactionViewModel.Date;
                income.Description = transactionViewModel.Description;
                
                _incomeRepo.UpdateIncome(income);

                transactionViewModel.Categories = categories;
                return RedirectToAction("Index");
            }
        }
        else if (spending != null)
        {
            if (ModelState.IsValid)
            {
                spending.Amount = transactionViewModel.Amount;
                spending.CategoryId = transactionViewModel.CategoryId;
                spending.Date = transactionViewModel.Date;
                spending.Description = transactionViewModel.Description;

                _spendingRepo.UpdateSpending(spending);

                transactionViewModel.Categories = categories;
                return RedirectToAction("Index");
            }
        }
        else
        {
            // Transaction not found
            return NotFound();
        }
    }

    // If we got this far, something went wrong, redisplay form
    transactionViewModel.Categories = categories;
    return View(transactionViewModel);
}


private async Task PopulateCategories()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        throw new Exception("User is not authenticated.");
    }

    var categories = await _categoryRepo.GetAllCategoriesByUserId(user.Id);

    var categoryList = categories.Select(c => new
    {
        Id = c.Id,
        Name = c.Name
    }).ToList();
    ViewBag.CategoryList = new SelectList(categoryList, "Id", "Name");
}



}
