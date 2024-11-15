namespace Questlog.Application.Common.DTOs.Quest;

public class QuestStatDto
{
    public int CompletedQuests { get; set; }
    public int InProgressQuests { get; set; }
    public int PastDueQuests { get; set; }
}