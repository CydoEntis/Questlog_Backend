using Questlog.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class QuestBoard
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public int Order { get; set; }

    [Required]
    public string BoardColor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }

    [Required]
    public int MainQuestId { get; set; }

    [ForeignKey("MainQuestId")]
    public MainQuest MainQuest { get; set; }

    [Required]
    public string UserId { get; set; }

    public virtual List<Quest> Quests { get; set; } = new List<Quest>();
}
