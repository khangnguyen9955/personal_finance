using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;
public interface IIncomeRepository
{
    IQueryable<Income> GetAllIncomes(Guid userId);
    Task<double> GetTotalIncomes(Guid userId);

    Income GetIncomeById(Guid id);
    void AddIncome(Income income);
    void UpdateIncome(Income income);
    void DeleteIncome(Guid id);
    Task<double> GetTotalIncomeLast7DaysAsync(Guid userId);

}
