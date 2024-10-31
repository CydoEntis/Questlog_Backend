namespace Questlog.Application.Common.DTOs.Step;

public class UpdateStepDto
{
    public int? Id { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
    public int? QuestId { get; set; }
}