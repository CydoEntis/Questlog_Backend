using Questlog.Application.Common.DTOs.PartyMember;

namespace Questlog.Application.Common.DTOs.Party;

public record CreatePartyResponseDTO
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public int GuildId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<CreatePartyMemberResponseDTO> PartyMembers { get; set; } = new List<CreatePartyMemberResponseDTO>();
}
