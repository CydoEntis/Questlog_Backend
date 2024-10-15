using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Auth;

public record RefreshTokenRequestDTO
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }

}
