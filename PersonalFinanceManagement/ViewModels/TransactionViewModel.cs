using System.ComponentModel.DataAnnotations;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Validation;

namespace PersonalFinanceManagement.ViewModels;

public class TransactionViewModel
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
    public int CategoryId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid amount.")]
    public double Amount { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Please enter a description for this transaction.")]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
    public DateTime Date { get; set; }

    [TransactionType(ErrorMessage = "Please select a valid transaction type.")]
    public string TransactionType { get; set; } // "income" or "spending"
    public List<Category> Categories { get; set; }

}

