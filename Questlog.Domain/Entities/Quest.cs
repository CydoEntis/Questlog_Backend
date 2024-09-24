using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class Quest
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Desc { get; set; }
    public string[]? Items { get; set; }
    [Required]
    public bool Completed { get; set; }
    [Required]
    public string Priority { get; set; }
    [Required]
    public int Order { get; set; }
    
    [Required]
    public string QuestBoardId { get; set; }
    
    [ForeignKey("QuestBoardId")]
    public QuestBoard QuestBoard { get; set; }
}