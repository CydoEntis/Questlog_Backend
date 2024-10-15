namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record UpdateGuildMemberRequestDTO
{
    public int Id { get; set; }
    public string Role { get; set; }
}
