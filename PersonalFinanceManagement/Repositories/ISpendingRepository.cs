using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public interface ISpendingRepository
{
    IQueryable<Spending> GetAllSpendings(Guid userId);
    Spending GetSpendingById(Guid id);
    void AddSpending(Spending spending);
    void UpdateSpending(Spending spending);
    void DeleteSpending(Guid id);
    Task<double> GetTotalSpendingLast7DaysAsync(Guid userId);

}
