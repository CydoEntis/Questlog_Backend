using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.PartyMember;

public record PartyMemberResponseDTO
{
    public int Id { get; set; }
    public int PartyId { get; set; }
    public int GuildId { get; set; }
    public int GuildMemberId { get; set; }
    public string Role { get; set; }
    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
}
