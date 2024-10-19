using System.ComponentModel.DataAnnotations;

namespace Questlog.Application.Common.DTOs.Campaign.Requests;

public record UpdateCampaignDetailsRequestDto
{
    [Required(ErrorMessage = "Campaign id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campaign name is required")]
    [MinLength(3, ErrorMessage = "Campaign name must be at least 3 characters")]
    [MaxLength(20, ErrorMessage = "Campaign name cannot exceed 20 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Campaign description is required")]
    [MinLength(5, ErrorMessage = "Campaign description must be at least 5 characters")]
    [MaxLength(50, ErrorMessage = "Campaign description cannot exceed 50 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Campaign color is required")]
    public required string Color { get; set; }
}
