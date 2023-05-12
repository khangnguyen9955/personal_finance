using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;
public interface IIncomeRepository
{
    IEnumerable<Income> GetAllIncomes();
    Income GetIncomeById(int id);
    void AddIncome(Income income);
    void UpdateIncome(Income income);
    void DeleteIncome(int id);
    Task<double> GetTotalIncomeLast7DaysAsync();

}
