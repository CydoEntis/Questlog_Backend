using Questlog.Application.Common.DTOs.Quest;

namespace Questlog.Application.Common.DTOs.User;

public class UserStatsDto
{
    public int TotalParties { get; set; }
    public int TotalQuests { get; set; }
    public int CompletedQuests { get; set; }
    public int InProgressQuests { get; set; }
    public int PastDueQuests { get; set; }

    public List<QuestCompletionOverTimeDto> QuestsCompletedCurrentMonth { get; set; }

    // Detailed statistics for quests completed by day
    public List<QuestCompletionByDayDto> QuestsCompletedByDay { get; set; }
}