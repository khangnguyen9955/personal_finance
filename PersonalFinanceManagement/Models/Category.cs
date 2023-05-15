using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; } // Foreign key property

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public ICollection<Spending> Spendings { get; set; } = new List<Spending>();

        public ICollection<Income> Incomes { get; set; } = new List<Income>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }  = DateTime.UtcNow;
    }
}