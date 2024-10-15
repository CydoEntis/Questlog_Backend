using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Party;

public record CreatePartyRequestDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }
    [Required]
    public int GuildId { get; set; }
}
