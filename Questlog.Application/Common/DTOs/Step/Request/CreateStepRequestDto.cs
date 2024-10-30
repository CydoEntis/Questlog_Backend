namespace Questlog.Application.Common.DTOs.Task.Request;

public record CreateStepRequestDto()
{
    public string Description { get; set; }
}