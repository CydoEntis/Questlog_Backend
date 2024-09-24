using System.ComponentModel.DataAnnotations;

namespace Questlog.Domain.Entities;

public class MainQuest
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public List<string> QuestBoard { get; set; }
    [Required]
    public string QuestColor { get; set; }
    [Required]
    public int Order { get; set; }

    public List<QuestBoard> QuestBoards { get; set; }
}