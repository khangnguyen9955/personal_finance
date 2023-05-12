namespace PersonalFinanceManagement.Models;

public interface ITransaction
{
    int Id { get; set; }
    string Description { get; set; }
    double Amount { get; set; }
    DateTime Date { get; set; }
    int CategoryId { get; set; }
    Category Category { get; set; }
    string UserId { get; set; }
    User User { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}
