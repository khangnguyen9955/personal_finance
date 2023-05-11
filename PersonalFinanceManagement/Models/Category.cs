using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public string UserId { get; set; } // Foreign key property

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }


    public ICollection<Spending> Spendings { get; set; } = new List<Spending>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } // Nullable
}