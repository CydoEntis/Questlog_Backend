namespace Questlog.Application.Common.DTOs.Task.Request;

public record CreateTaskRequestDto()
{
    public string Description { get; set; }
}