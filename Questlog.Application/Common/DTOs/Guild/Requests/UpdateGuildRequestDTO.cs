using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Guild.Requests;

public record UpdateGuildRequestDTO
{
    [Required]
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(25)]
    public string? Name { get; set; }

    [MinLength(5)]
    [MaxLength(100)]
    public string? Description { get; set; }

    public string Color { get; set; }
}
