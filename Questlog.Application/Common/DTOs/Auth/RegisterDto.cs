using Questlog.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Questlog.Application.Common.DTOs.Avatar;

namespace Questlog.Application.Common.DTOs.Auth;

public record RegisterDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Display Name is required.")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "Avatar is required.")]
    public AvatarDto Avatar { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
