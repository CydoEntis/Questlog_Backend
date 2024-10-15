namespace Questlog.Application.Common.DTOs.PartyMember;

public record CreatePartyMemberResponseDTO
{
    public int Id { get; set; }
    public int GuildId { get; set; }
    public int PartyId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; }
}
