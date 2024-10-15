using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questlog.Domain.Entities;

public class Party
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Primary key

    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }

    [ForeignKey("Guild")]
    public int GuildId { get; set; } 

    public Guild Guild { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<PartyMember> PartyMembers { get; set; } = new List<PartyMember>(); 

}

