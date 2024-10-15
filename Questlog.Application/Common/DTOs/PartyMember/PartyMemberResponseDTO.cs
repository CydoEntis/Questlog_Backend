using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.PartyMember;

public record PartyMemberResponseDTO
{
    [Required]
    public int Id { get; set; }

    public int CharacterId { get; set; }

    [Required]
    public int PartyId { get; set; }

    [Required]
    public IdentityRole Role { get; set; }

    [Required]
    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
}
