using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Auth;

public record LoginResponseDTO
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public TokenDTO Tokens { get; set; }
    public Avatar Avatar { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentExp { get; set; }
    public int ExpToNextLevel { get; set; }
}
