using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PersonalFinanceManagement.Models;

public class Spending : ITransaction
{
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The description field is required.")]
        [StringLength(50, ErrorMessage = "The description field must be a maximum of 50 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The amount field is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The amount field must be a positive number.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "The date field is required.")]
        public DateTime Date { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public Guid UserId { get; set; } // Foreign key property

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable

      
}