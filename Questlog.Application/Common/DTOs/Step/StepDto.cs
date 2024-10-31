namespace Questlog.Application.Common.DTOs.Step;

public class StepDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int QuestId { get; set; }
    public DateTime CreatedAt { get; set; } = default(DateTime);
    public DateTime UpdatedAt { get; set; } = default(DateTime);
}