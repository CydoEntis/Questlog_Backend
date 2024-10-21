namespace Questlog.Application.Common.DTOs.Subquest.Request;

public record CreateSubquestRequestDto()
{
    public string Description { get; set; }
    public int QuestId { get; set; }

}