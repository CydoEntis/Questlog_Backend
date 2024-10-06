using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questlog.Domain.Entities;

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
        public int CurrentLevel { get; set; } = 1;

        [Required]
        public int CurrentExp { get; set; } = 0;

        [Required]
        public int ExpToNextLevel { get; set; } = 100;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public Character()
        {
            CurrentLevel = 1;
            CurrentExp = 0;
        }

        public int CalculateExpForLevel()
        {
            int baseExp = 100;
            return baseExp * CurrentLevel;
        }
    }
}

