using System.ComponentModel.DataAnnotations;

namespace InstructorInformationSystem.Models;

public class UserAccount
{
    public int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(50)]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Admin ID must contain numbers only.")]
    [Display(Name = "Admin ID")]
    public string AdminId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Stores a hash, never the plain-text password.
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}