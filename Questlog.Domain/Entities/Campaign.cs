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
    [MaxLength(25)]
    public string Name { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Description { get; set; }

    public string Color { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

    [ForeignKey("ApplicationUser")]
    public string OwnerId { get; set; } // Foreign key

    public ApplicationUser Owner { get; set; } // Navigation property
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}

