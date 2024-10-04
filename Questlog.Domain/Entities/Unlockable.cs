using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questlog.Domain.Entities
{
    public class Unlockable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        [Range(5, 5000, ErrorMessage = "Cost must be at least 5, but no more than 5000")]
        public int Cost { get; set; }
        [Required]
        public Archetype Archetype { get; set; }

        [ForeignKey("Character")]
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}