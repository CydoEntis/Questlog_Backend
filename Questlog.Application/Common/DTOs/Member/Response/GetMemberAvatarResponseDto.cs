using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record GetMemberAvatarResponseDto()
{
    public int Id { get; set; }
    public Avatar Avatar { get; set; }
    public string DisplayName { get; set; }
}