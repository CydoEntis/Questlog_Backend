namespace Questlog.Application.Common.DTOs.Campaign.Requests;

public record UpdateCampaignRequestDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public DateTime? DueDate { get; set; }
}
