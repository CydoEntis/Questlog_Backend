
using Questlog.Application.Common.DTOs.Avatar;

namespace Questlog.Application.Common.DTOs.User;

public class UserDto
{
    public string DisplayName { get; set; }
    public AvatarDto Avatar { get; set; }
    public int CurrentLevel { get; set; } = 1;
    public int CurrentExp { get; set; } = 0;
    public int Currency { get; set; } = 0;
    public int ExpToNextLevel { get; set; } = 100;
}