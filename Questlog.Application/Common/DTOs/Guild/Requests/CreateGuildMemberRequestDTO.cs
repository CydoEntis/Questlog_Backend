using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record CreateGuildMemberRequestDTO
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public int GuildId { get; set; }
}
