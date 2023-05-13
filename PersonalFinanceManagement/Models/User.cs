using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceManagement.Models;

public class User : IdentityUser<Guid>
{

    [Range(0, double.MaxValue, ErrorMessage = "The balance field must be a positive number.")]
    public double Balance { get; set; }

    // Added fields for audit trail
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } // Nullable

    public ICollection<Category> Categories { get; set; }
    public ICollection<Spending> Spendings { get; set; }
    public ICollection<Income> Incomes { get; set; }
}