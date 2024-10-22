using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Member.Response;

public record GetMemberAvatarResponseDto()
{
    public int Id { get; set; }
    public Avatar Avatar { get; set; }
    public string DisplayName { get; set; }
}