using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceManagement.Models;

public class User : IdentityUser
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The email field is required.")]
    [EmailAddress(ErrorMessage = "The email field must be a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The password field is required.")]
    [MinLength(6, ErrorMessage = "The password field must be at least 6 characters long.")]
    public string Password { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "The balance field must be a positive number.")]
    public double Balance { get; set; }



    // Added fields for audit trail
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } // Nullable
    
    
    public ICollection<Category> Categories { get; set; }

    public ICollection<Spending> Spendings { get; set; }
    public ICollection<Income> Incomes { get; set; }
}