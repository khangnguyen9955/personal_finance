using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "The email field is required.")]
    [EmailAddress(ErrorMessage = "The email field must be a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The password field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

}