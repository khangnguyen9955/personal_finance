using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Validation;
public class TransactionTypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string transactionType = value?.ToString();
        if (transactionType != "Income" && transactionType != "Spending")
        {
            return new ValidationResult("Invalid transaction type.");
        }
        return ValidationResult.Success;
    }
}