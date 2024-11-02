namespace Questlog.Application.Common.DTOs.Auth;

public record LoginCredentialsDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
