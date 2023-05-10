using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;
public class IncomeController: Controller
{
    private readonly IIncomeRepository _incomeRepo;

    public IncomeController(IIncomeRepository incomeRepo)
    {
        _incomeRepo = incomeRepo;
    }

    // public IActionResult Index()
    // {
    //     var incomes = _incomeRepo.GetAllIncomes();
    //     return View(incomes);
    // }

    //...
}