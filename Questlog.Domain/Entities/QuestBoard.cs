using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

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
    
    [Required]
    public int MainQuestId { get; set; }
    
    [ForeignKey("MainQuestId")]
    public MainQuest MainQuest { get; set; }
    
    public List<Quest> Quests { get; set; }
    
}