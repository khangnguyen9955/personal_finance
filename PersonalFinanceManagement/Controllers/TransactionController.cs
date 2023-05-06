using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;
using PersonalFinanceManagement.ViewModels;

namespace PersonalFinanceManagement.Controllers;

public class TransactionController :  Controller
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IIncomeRepository _incomeRepo;
    private readonly ISpendingRepository _spendingRepo;

    public TransactionController(ICategoryRepository categoryRepo, IIncomeRepository incomeRepo, ISpendingRepository spendingRepo)
    {
        _categoryRepo = categoryRepo;
        _incomeRepo = incomeRepo;
        _spendingRepo = spendingRepo;
    }

    public IActionResult Index()
    {
        var incomes = _incomeRepo.GetAllIncomes().ToList(); // convert to list
        var spendings = _spendingRepo.GetAllSpendings().ToList();
        var viewModel = new TransactionViewModel 
        {
            Incomes = incomes,
            Spendings = spendings,
            Categories = _categoryRepo.GetAllCategories().ToList()
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new TransactionViewModel
        {
            Categories = _categoryRepo.GetAllCategories().ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(double amount, string description, int categoryId, DateTime date, string transactionType)
    {
        if (transactionType == "income")
        {
            var income = new Income
            {
                Amount = amount,
                Description = description,
                Category = _categoryRepo.GetCategoryById(categoryId),
                Date = date
            };
            _incomeRepo.AddIncome(income);
        }
        else if (transactionType == "spending")
        {
            var spending = new Spending 
            {
                Amount = amount,
                Description = description,
                Category =_categoryRepo.GetCategoryById(categoryId),
                Date = date
            };
            _spendingRepo.AddSpending(spending);
        }
        return RedirectToAction(nameof(Index));
    }
}
