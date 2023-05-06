using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.ViewModels;

public class TransactionViewModel
{
    public List<Income> Incomes { get; set; }
    public List<Spending> Spendings { get; set; }
    public List<Category> Categories { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalSpending { get; set; }
    public decimal TotalProfit { get { return TotalIncome - TotalSpending; } }
}
