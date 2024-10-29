using Questlog.Domain.Entities;

namespace Questlog.Application.Common.DTOs.Member.Response;

public record GetSimpleMemberResponseDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; }
    public Avatar Avatar { get; set; }
}