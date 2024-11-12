namespace Questlog.Application.Common.DTOs.User;

public class UserStatsDto
{
    public int TotalParties { get; set; }
    public int TotalQuests { get; set; }
    public int CompletedQuests { get; set; }
    public int InProgressQuests { get; set; }
    public int PastDueQuests { get; set; }
}