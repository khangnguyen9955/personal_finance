using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.ViewModels;

public class CategoryViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(20, ErrorMessage = "The Name field cannot exceed 10 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Type field is required.")]
    public string Type { get; set; }
}