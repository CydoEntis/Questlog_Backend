namespace Questlog.Application.Common.DTOs.Task.Request;

public record UpdateStepsRequestDto()
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int QuestId { get; set; }
}