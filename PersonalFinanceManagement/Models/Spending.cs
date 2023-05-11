using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Models;

public class Spending
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The description field is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The amount field is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "The amount field must be a positive number.")]
    public double Amount { get; set; }

    [Required(ErrorMessage = "The date field is required.")]
    public DateTime Date { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    
    public string UserId { get; set; } // Foreign key property

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } // Nullable
}