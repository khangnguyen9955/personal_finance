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
    public async Task<IActionResult> CreateOrEditTransaction(int id = 0)
    {
        var viewModel = new TransactionViewModel
        {
            Categories = await _categoryRepo.GetAllCategories().AsQueryable().ToListAsync();

        };

        if (id != 0)
        {
            var spending = await _spendingRepo.GetSpendingByIdAsync(id);
            if (spending != null)
            {
                viewModel.Id = spending.Id;
                viewModel.CategoryId = spending.CategoryId;
                viewModel.Amount = -spending.Amount; // negative amounts represent spending records
                viewModel.Description = spending.Description;
                viewModel.Date = spending.Date;
                viewModel.TransactionType = "spending";
            }
            else
            {
                var income = await _incomeRepo.GetIncomeByIdAsync(id);
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

        await PopulateCategories();
        return View(viewModel);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionViewModel transactionViewModel)
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
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
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
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                // add spending to repository
                _spendingRepo.AddSpending(spending);
            }
            else
            {
                return BadRequest("Invalid transaction type.");
            }
            transactionViewModel.Categories = _categoryRepo.GetAllCategories().ToList();
            return RedirectToAction("Index");
        }

        // If we got this far, something went wrong, redisplay form
        return View(transactionViewModel);

    }
    private async Task PopulateCategories()
    {
        var user = await _userManager.GetUserAsync(User);
        var categories = _categoryRepo.GetAllCategories()
            .Where(c => c.UserId == user.Id)
            .Select(c => new { Id = c.Id, Name = c.Name })
            .ToList();
        ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
    }


}
