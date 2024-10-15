namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record UpdateGuildLeaderRequestDTO
{
    public int GuildId { get; set; }
    public string UserId { get; set; }
}
