using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Domain.Entities
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(12)]
        [MinLength(3)]
        public string DisplayName { get; set; }
        [Required]
        public Archetype Archetype { get; set; }
        [Required]
        public int CurrentExp { get; set; } = 0;
        [Required]
        public int ExpToNextLevel { get; set; } = 100;

        public List<Unlockable> Inventory { get; set; } = new List<Unlockable>();

        public ApplicationUser User { get; set; }
    }
}
