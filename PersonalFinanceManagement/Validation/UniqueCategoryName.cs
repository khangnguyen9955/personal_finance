using System.ComponentModel.DataAnnotations;
using PersonalFinanceManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace PersonalFinanceManagement.Validation
{
    public class UniqueCategoryNameAttribute : ValidationAttribute
    {
        private readonly  MyDbContext _context;

        public UniqueCategoryNameAttribute(MyDbContext context)
        {
            _context = context;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var nameWithSpaces = value?.ToString();
            var name = nameWithSpaces?.Trim();

            if (string.IsNullOrEmpty(name) || name != nameWithSpaces)
            {
                return new ValidationResult($"{validationContext.DisplayName} must not be empty, spaces only, or leading/trailing spaces.");
            }

            var userId = ((Category)validationContext.ObjectInstance).UserId;
            var category = _context.Categories
                .AsNoTracking() // To access the table without tracking changes
                .SingleOrDefault(c => c.UserId == userId && c.Name.Trim().Equals(name)); // Case-sensitive comparison without leading/trailing spaces

            if (category != null && category.Id != ((Category)validationContext.ObjectInstance).Id)
            {
                return new ValidationResult($"A category with the name '{name}' already exists for this user.");
            }

            return ValidationResult.Success;
        }
    }
}
