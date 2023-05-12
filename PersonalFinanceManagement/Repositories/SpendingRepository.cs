using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public class SpendingRepository : ISpendingRepository
{
    private readonly MyDbContext _context;

    public SpendingRepository(MyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Spending> GetAllSpendings()
    {
        return _context.Spendings.ToList();
    }

    public Spending GetSpendingById(int id)
    {
        return _context.Spendings.FirstOrDefault(s => s.Id == id);
    }

    public void AddSpending(Spending spending)
    {
        _context.Spendings.Add(spending);
        _context.SaveChanges();
    }

    public void UpdateSpending(Spending spending)
    {
        _context.Spendings.Update(spending);
        _context.SaveChanges();
    }

    public void DeleteSpending(int id)
    {
        var spending = _context.Spendings.Find(id);
        if (spending != null)
        {
            _context.Spendings.Remove(spending);
            _context.SaveChanges();
        }
    }
    public async Task<double> GetTotalSpendingLast7DaysAsync()
    {
        // Get DateTime object for 7 days ago
        var sevenDaysAgo = DateTime.Today.AddDays(-7);

        // Calculate total spending for last 7 days
        var totalSpending = await _context.Spendings
            .Where(s => s.Date >= sevenDaysAgo)
            .SumAsync(s => s.Amount);

        return totalSpending;
    }
}
