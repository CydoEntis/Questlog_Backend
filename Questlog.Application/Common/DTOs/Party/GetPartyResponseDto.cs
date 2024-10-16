using Questlog.Application.Common.DTOs.PartyMember;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Party;

public record GetPartyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GuildId { get; set; }
    public string PartyLeaderId { get; set; }
    public string PartyLeader { get; set; }
    public string Color { get; set; }
    public int NumberOfMembers { get; set; }
    public DateTime CreatedAt { get; set; }
}
