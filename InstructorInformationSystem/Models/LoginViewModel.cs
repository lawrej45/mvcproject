using System.ComponentModel.DataAnnotations;

namespace InstructorInformationSystem.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your Email or Admin ID.")]
    [Display(Name = "Email or Admin ID")]
    public string EmailOrId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}