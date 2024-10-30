namespace Questlog.Domain.Entities;

public class Step
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int QuestId { get; set; }
    public Quest Quest { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}