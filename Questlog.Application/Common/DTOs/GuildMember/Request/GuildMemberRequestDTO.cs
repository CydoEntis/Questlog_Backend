namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record GuildMemberRequestDTO
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int GuildId { get; set; }
    public string Role { get; set; }
}
