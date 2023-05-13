using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;
public interface IIncomeRepository
{
    IEnumerable<Income> GetAllIncomes();
    Income GetIncomeById(Guid id);
    void AddIncome(Income income);
    void UpdateIncome(Income income);
    void DeleteIncome(Guid id);
    Task<double> GetTotalIncomeLast7DaysAsync();

}
