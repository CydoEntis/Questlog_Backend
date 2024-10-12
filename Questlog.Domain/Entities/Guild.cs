using Questlog.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Guild
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Description { get; set; }

    public string Color { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ApplicationUser")]
    public string GuildLeaderId { get; set; }

    public ApplicationUser GuildLeader { get; set; }

    public virtual ICollection<GuildMember> GuildMembers { get; set; } = new List<GuildMember>();

    public virtual ICollection<Party> Parties { get; set; } = new List<Party>();
}
