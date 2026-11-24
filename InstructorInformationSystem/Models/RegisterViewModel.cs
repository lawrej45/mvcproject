using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InstructorInformationSystem.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Admin ID is required.")]
    [StringLength(50, ErrorMessage = "Admin ID cannot exceed 50 characters.")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Admin ID must contain numbers only.")]
    [Remote("IsAdminIdAvailable", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken", ErrorMessage = "This Admin ID is already registered.")]
    [Display(Name = "Admin ID")]
    public string AdminId { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
    [Remote("IsEmailAvailable", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken", ErrorMessage = "An account with this email already exists.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}