namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record UpdateMemberRoleRequestDto
{
    public int Id { get; set; }
    public string Role { get; set; }
}
