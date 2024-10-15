namespace Questlog.Application.Common.DTOs.Auth;

public record TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
