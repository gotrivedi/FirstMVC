using System.ComponentModel.DataAnnotations;

namespace UserManagment.Models;

public class User
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a valid name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(
    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
    ErrorMessage = "Please enter a valid email address"
)]
    public string Email { get; set; } = string.Empty;
}