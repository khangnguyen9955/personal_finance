using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly MyDbContext _context;

    public IncomeRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Income> GetAllIncomes()
    {
        return _context.Incomes.ToList();
    }

    public Income GetIncomeById(int id)
    {
        return _context.Incomes.FirstOrDefault(i => i.Id == id);
    }

    public void AddIncome(Income income)
    {
        _context.Incomes.Add(income);
        _context.SaveChanges();
    }

    public void UpdateIncome(Income income)
    {
        _context.Incomes.Update(income);
        _context.SaveChanges();
    }

    public void DeleteIncome(int id)
    {
        var income = _context.Incomes.Find(id);
        if (income != null)
        {
            _context.Incomes.Remove(income);
            _context.SaveChanges();
        }
    }
    public async Task<double> GetTotalIncomeLast7DaysAsync()
    {
        // Get DateTime object for 7 days ago
        var sevenDaysAgo = DateTime.Today.AddDays(-7);

        // Calculate total income for last 7 days
        var totalIncome = await _context.Incomes
            .Where(i => i.Date >= sevenDaysAgo)
            .SumAsync(i => i.Amount);

        return totalIncome;
    }
}
