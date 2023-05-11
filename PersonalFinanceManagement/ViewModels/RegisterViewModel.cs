using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.ViewModels;


public class RegisterViewModel
{
    [Required(ErrorMessage = "The name field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The email field is required.")]
    [EmailAddress(ErrorMessage = "The email field must be a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The password field is required.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "The password field must be at least 6 characters long.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "The Confirm password field is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}