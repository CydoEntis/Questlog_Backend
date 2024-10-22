namespace Questlog.Application.Common.DTOs.Member.Request;

public record UpdateMemberRoleRequestDto
{
    public int Id { get; set; }
    public string Role { get; set; }
}