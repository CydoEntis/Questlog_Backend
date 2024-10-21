namespace Questlog.Domain.Entities;

public class Subquest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool isCompleted { get; set; }
    public int QuestId { get; set; }
    public Quest Quest { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}