using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;

public class SpendingController: Controller 
{
    private readonly ISpendingRepository _spendingRepo;

    public SpendingController(ISpendingRepository spendingRepo)
    {
        _spendingRepo = spendingRepo;
    }

    public IActionResult Index()
    {
        var spendings = _spendingRepo.GetAllSpendings();
        return View(spendings);
    }

    //...
}