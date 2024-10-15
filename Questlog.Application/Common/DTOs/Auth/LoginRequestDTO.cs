namespace Questlog.Application.Common.DTOs.Auth;

public record LoginRequestDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}
