namespace Questlog.Application.Common.DTOs.GuildMember.Response;

public record GuildMemberResponseDTO
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int GuildId { get; set; }
    public string Role { get; set; }
    public DateTime JoinedOn { get; set; }

}
