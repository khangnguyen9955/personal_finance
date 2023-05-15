using System.ComponentModel.DataAnnotations;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Validation;

namespace PersonalFinanceManagement.ViewModels;

public class TransactionViewModel
{
    public Guid Id { get; set; }

    [Range(typeof(Guid), "00000000-0000-0000-0000-000000000000", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", ErrorMessage = "Please select a valid category.")]
    public Guid CategoryId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid amount.")]
    public double Amount { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Please enter a description for this transaction.")]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    


    public string? TransactionType { get; set; } // "income" or "spending"
    public List<Category>? Categories { get; set; }
    public string? CategoryName { get; set; }
    public List<Spending>? Spendings { get; set; }
    public List<Income>? Incomes { get; set; }
    public List<TransactionViewModel>? Transactions { get; set; } // Child transactions (sub-categories or individual transactions)


}

