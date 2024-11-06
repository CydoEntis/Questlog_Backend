namespace Questlog.Application.Common.DTOs.Avatar;

public class UnlockedAvatarDto
{
    public int AvatarId { get; set; }
    public AvatarDto Avatar { get; set; }
    public DateTime UnlockedAt { get; set; }
}