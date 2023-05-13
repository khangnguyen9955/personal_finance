namespace PersonalFinanceManagement.Models;

public interface ITransaction
{
    Guid Id { get; set; }
    string Description { get; set; }
    double Amount { get; set; }
    DateTime Date { get; set; }
    Guid CategoryId { get; set; }
    Category Category { get; set; }
    Guid UserId { get; set; }
    User User { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}
