using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Auth;

public record LoginDto
{
    // public string UserId { get; set; }
    // public string Email { get; set; }
    public string DisplayName { get; set; }
    public TokenDTO Tokens { get; set; }
    public AvatarDto Avatar { get; set; }
    public int Currency { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentExp { get; set; }
    public int ExpToNextLevel { get; set; }
}