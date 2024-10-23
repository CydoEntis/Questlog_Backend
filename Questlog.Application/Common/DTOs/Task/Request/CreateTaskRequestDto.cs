namespace Questlog.Application.Common.DTOs.Subquest.Request;

public record CreateTaskRequestDto()
{
    public string Description { get; set; }
}