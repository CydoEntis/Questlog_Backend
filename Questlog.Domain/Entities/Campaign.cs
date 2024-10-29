using Questlog.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Domain.Entities;

public class Campaign
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Title { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(255)]
    public string Description { get; set; }

    public string Color { get; set; } = "blue";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

    [ForeignKey("ApplicationUser")]
    public string OwnerId { get; set; }

    public ApplicationUser Owner { get; set; } 
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
    
    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
}

