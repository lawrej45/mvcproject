using System.ComponentModel.DataAnnotations;

namespace InstructorInformationSystem.Models;

public class Instructor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Instructor ID is required.")]
    [StringLength(20, ErrorMessage = "Instructor ID cannot exceed 20 characters.")]
    [RegularExpression("^(\\d+(-\\d+)*|N/A)$", ErrorMessage = "Instructor ID must be numbers (e.g., 24-002) or N/A.")]
    [Display(Name = "Instructor ID")]
    public string InstructorId { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [RegularExpression("^[^@\\d]+$", ErrorMessage = "First name must not contain numbers or an email address.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    [RegularExpression("^[^@\\d]+$", ErrorMessage = "Last name must not contain numbers or an email address.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    // Built from FirstName + LastName by the controller; not entered on the forms.
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age is required.")]
    [Range(20, 80, ErrorMessage = "Age must be between 20 and 80.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required.")]
    [StringLength(100, ErrorMessage = "Designation cannot exceed 100 characters.")]
    public string Designation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required.")]
    [RegularExpression("^(Male|Female|Prefer not to say)$", ErrorMessage = "Select Male, Female, or Prefer not to say.")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact number is required.")]
    [RegularExpression("^(N/A|[0-9+()\\- ]{7,20})$", ErrorMessage = "Enter a valid contact number or N/A.")]
    [Display(Name = "Contact Number")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Majorship is required.")]
    [StringLength(100, ErrorMessage = "Majorship cannot exceed 100 characters.")]
    public string Majorship { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bachelor's degree is required.")]
    [StringLength(150, ErrorMessage = "Bachelor's degree cannot exceed 150 characters.")]
    [Display(Name = "Bachelor Degree")]
    public string BachelorDegree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
    [Display(Name = "E-mail Address")]
    public string EmailAddress { get; set; } = string.Empty;
}