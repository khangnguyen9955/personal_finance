using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name field is required.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "The name field is required.")]
    public string Type { get; set; } = "Expense";
    public int UserId { get; set; }

    public ICollection<Spending>? Spendings { get; set; }
    public ICollection<Income>? Incomes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } // Nullable 
}