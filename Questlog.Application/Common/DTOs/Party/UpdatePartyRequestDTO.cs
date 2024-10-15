using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Party;

public record UpdatePartyRequestDTO
{
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }
}
