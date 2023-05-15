using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;
using PersonalFinanceManagement.ViewModels;


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
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var spendings = await _spendingRepo.GetAllSpendings(user.Id).ToListAsync();
        var incomes = await _incomeRepo.GetAllIncomes(user.Id).ToListAsync();


        var transactions = new List<ITransaction>();
        transactions.AddRange(spendings);
        transactions.AddRange(incomes);
        transactions = transactions.OrderByDescending(t => t.Date).ToList();
        var transactionViewModels = transactions.Select(t => new TransactionViewModel
        {
            Id = t.Id,
            CategoryId = t.CategoryId,
            Amount = t.Amount,
            Description = t.Description,
            Date = t.Date,
            TransactionType = t.Category.Type,
            CategoryName = t.Category.Name // Assuming the category has a 'Name' property
        }).ToList();

        var transactionViewModel = new TransactionViewModel
        {
            Transactions = transactionViewModels
        };

        return View(transactionViewModel);
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
            Categories = categories,
            Date = DateTime.Today
        };

        if (id == Guid.Empty) // Create new transaction
        {
            return View(viewModel);
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
    var category = _categoryRepo.GetCategoryById(transactionViewModel.CategoryId);

    if (id == Guid.Empty )// Create new transaction
    {
        transactionViewModel.TransactionType = "";
        transactionViewModel.Categories = categories;
        if (ModelState.IsValid)
        {
            if (category.Type == "Income")
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
            else if (category.Type == "Spending")
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


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Delete(Guid id)
{
    // Determine whether the transaction being deleted is an income or a spending
    var income = _incomeRepo.GetIncomeById(id);
    var spending = _spendingRepo.GetSpendingById(id);

    if (income != null)
    {
        _incomeRepo.DeleteIncome(income.Id);
    }
    else if (spending != null)
    {
        _spendingRepo.DeleteSpending(spending.Id);
    }
    else
    {
        return NotFound();
    }

    return RedirectToAction("Index");
}


}
