using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public interface ISpendingRepository
{
    IEnumerable<Spending> GetAllSpendings();
    Spending GetSpendingById(int id);
    void AddSpending(Spending spending);
    void UpdateSpending(Spending spending);
    void DeleteSpending(int id);
    Task<double> GetTotalSpendingLast7DaysAsync();

}
